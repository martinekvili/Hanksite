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

        #region counter
        public bool IsCounterRunning { get; set; }
        public int RemainingSeconds { get; set; }
        public Brush CounterColour { get; set; }
        private SolidColorBrush defaultColour;
        private SolidColorBrush lastSecondsColour;
        private SoundPlayer soundPlayer;
        #endregion

        public Common.Game.Player[] Players { get; set; }

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

        public void ChooseColor(Brush brush)
        {
            gameServer.ChooseColour(DecodeColor(brush));
        }

        private int DecodeColor(Brush brush)
        {
            return colours.FirstOrDefault(x => x.Value == ((SolidColorBrush)brush).Color).Key;
        }

        #region callbacks
        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            RefreshGame(snapshot);
        }

        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            RefreshGame(snapshot);
            StartCounter();
        }

        public void SendGamePlayerSnapshot(GamePlayersSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            RefreshGame(snapshot);

        }
        #endregion

        private void RefreshGame(GameSnapshot snapshot)
        {
            List<Field> fields = mapConverter.ConvertToFields(snapshot.Map);
            map = mapConverter.ConvertToDrawable(fields, CanvasWidth, CanvasHeight);
            NotifyPropertyChanged("Map");

            Players = snapshot.Players;
            NotifyPropertyChanged("Players");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
