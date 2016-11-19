using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using Client.ViewModel.Interfaces;
using Common.Game;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Media;
using System.Windows.Resources;
using Client.Properties;
using System.Windows.Navigation;
using Client.View;

namespace Client.ViewModel
{
    class GameViewModel : INotifyPropertyChanged, IGameActions
    {
        public ICommand TestCommand { get; set; }

        private MapProvider mapProvider;
        private Dictionary<int, Color> colours;
        private MapConverter mapConverter;

        public float CanvasWidth { get; set; }
        public float CanvasHeight { get; set; }

        private List<DrawableField> map;
        public ObservableCollection<DrawableField> Map => new ObservableCollection<DrawableField>(map);

        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        #region counter
        public bool IsCounterRunning { get; set; }
        public int RemainingSeconds { get; set; }
        public Brush CounterColour { get; set; }
        private SolidColorBrush defaultColour;
        private SolidColorBrush lastSecondsColour;
        private SoundPlayer soundPlayer;
        #endregion

        public List<GamePlayer> Players { get; set; }
        public List<Coordinate> AvailableCells { get; set; }

        private IGameServer gameServer;

        public GameViewModel()
        {
            mapProvider = new MapProvider();
            colours = new ColourProvider().Colours;
            mapConverter = new MapConverter();

            CanvasWidth = 800;
            CanvasHeight = 650;

            map = mapConverter.ConvertToDrawable(mapProvider.GetMap(), CanvasWidth, CanvasHeight);

            #region counter
            IsCounterRunning = false;
            defaultColour = new SolidColorBrush(Color.FromScRgb(1, 0, 0, 0));
            lastSecondsColour = new SolidColorBrush(Color.FromScRgb(1, 1, 0, 0));
            soundPlayer = new SoundPlayer(Resources.lastseconds);
            #endregion

            TestCommand = new CommandHandler(Test);
        }

        private void Test()
        {
            StartCounter();
        }

        #region counter
        private void StartCounter()
        {
            RemainingSeconds = 15;
            NotifyPropertyChanged("RemainingSeconds");

            IsCounterRunning = true;
            NotifyPropertyChanged("IsCounterRunning");

            CounterColour = defaultColour;
            NotifyPropertyChanged("CounterColour");

            Task.Factory.StartNew(() =>
            {
                while (IsCounterRunning && 0 < RemainingSeconds)
                {
                    Thread.Sleep(1000);
                    RemainingSeconds--;
                    NotifyPropertyChanged("RemainingSeconds");

                    if (RemainingSeconds <= 3)
                    {
                        CounterColour = lastSecondsColour;
                        NotifyPropertyChanged("CounterColour");

                        soundPlayer.Play();
                    }
                }

                IsCounterRunning = false;
                NotifyPropertyChanged("IsCounterRunning");
            });
        }
        #endregion

        public void ChooseColour(float x, float y)
        {
            DrawableField chosenField = map.FirstOrDefault(field => field.X == x && field.Y == y);
            if (AvailableCells.FirstOrDefault(cell => cell.X == chosenField.LogicalPosition.X && cell.Y == chosenField.LogicalPosition.Y) == null)
            {
                return;
            }

            int chosenColour = DecodeColur(chosenField.Colour);
            IsCounterRunning = false;
            gameServer.ChooseColour(chosenColour);
        }

        private int DecodeColur(Brush brush)
        {
            return colours.FirstOrDefault(x => x.Value == ((SolidColorBrush)brush).Color).Key;
        }

        #region callbacks
        public void SendGameSnapshot(GameState state)
        {
            RefreshGame(state);
        }

        public void DoNextStep(GameState state, List<Coordinate> availableCells)
        {
            AvailableCells = availableCells;
            RefreshGame(state);
            StartCounter();
        }

        public void SendGamePlayerSnapshot(List<GamePlayer> players)
        {
            Players = players;
            NotifyPropertyChanged("Players");
        }

        public void SendGameOver(GameState state)
        {
            RefreshGame(state);
            MessageBox.Show("Game Over", "Hanksite", MessageBoxButton.OK);
            gameServer.DisconnectFromGame();
            NavigationService.GetNavigationService(View).Navigate(new MainMenu());
        }
        #endregion

        private void RefreshGame(GameState state)
        {
            map = mapConverter.ConvertToDrawable(state.Map, CanvasWidth, CanvasHeight);
            NotifyPropertyChanged("Map");

            Players = state.Players;
            NotifyPropertyChanged("Players");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
