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
using Client.ServerConnection;
using System.Windows.Data;
using System.Globalization;
using Client.View.Dialogs;

namespace Client.ViewModel
{
    class GameViewModel : INotifyPropertyChanged, IGameActions
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private Dictionary<int, Color> colours;
        private MapConverter mapConverter;
        private MapAttributes mapAttributes;

        public float CanvasWidth { get; set; }
        public float CanvasHeight { get; set; }

        #region fields
        private List<DrawableField> map;
        public ObservableCollection<DrawableField> Map => new ObservableCollection<DrawableField>(map);

        private List<DrawableField> availableFields;
        public List<DrawableField> AvailableFields
        {
            get { return availableFields; }
            set { availableFields = value; NotifyPropertyChanged(nameof(AvailableFields)); }
        }

        private List<DrawableField> enemyFields;
        public List<DrawableField> EnemyFields
        {
            get { return enemyFields; }
            set { enemyFields = value; NotifyPropertyChanged(nameof(EnemyFields)); }
        }

        private List<DrawableField> playerFields;
        public List<DrawableField> PlayerFields
        {
            get { return playerFields; }
            set { playerFields = value; NotifyPropertyChanged(nameof(PlayerFields)); }
        }
        #endregion

        #region counter
        public bool IsCounterRunning { get; set; }
        public int RemainingSeconds => remainingSecondsByRound[actualRound];
        public Brush CounterColour { get; set; }
        private SolidColorBrush defaultColour;
        private SolidColorBrush lastSecondsColour;
        private SoundPlayer counterSoundPlayer;
        private int actualRound;
        private Dictionary<int, int> remainingSecondsByRound;
        #endregion

        private List<GamePlayer> players;
        public List<GamePlayer> Players
        {
            get { return players; }
            set { players = value.OrderBy(player => player.Position).ToList(); NotifyPropertyChanged("Players"); }
        }

        private SoundPlayer nextTurnSoundPlayer;

        private IGameServer gameServer;

        public GameViewModel()
        {
            colours = new ColourProvider().Colours;
            mapConverter = new MapConverter();

            CanvasWidth = 800;
            CanvasHeight = 650;

            map = new List<DrawableField>();

            #region counter
            IsCounterRunning = false;
            defaultColour = new SolidColorBrush(Color.FromScRgb(1, 0, 0, 0));
            lastSecondsColour = new SolidColorBrush(Color.FromScRgb(1, 1, 0, 0));
            counterSoundPlayer = new SoundPlayer(Resources.lastseconds);
            actualRound = 0;
            remainingSecondsByRound = new Dictionary<int, int>();
            remainingSecondsByRound[0] = 0;
            #endregion

            nextTurnSoundPlayer = new SoundPlayer(Resources.nextturn);

            gameServer = ClientProxyManager.Instance;
            try
            {
                ClientProxyManager.Instance.RegisterGame(this);
            }
            catch (Exception)
            {
                MessageBox.Show("Connection lost.", "Hanksite", MessageBoxButton.OK);
                Application.Current.Shutdown();
                return;
            }
        }

        #region counter
        private void StartCounter()
        {
            remainingSecondsByRound[actualRound] = 15;
            NotifyPropertyChanged("RemainingSeconds");

            IsCounterRunning = true;
            NotifyPropertyChanged("IsCounterRunning");

            CounterColour = defaultColour;
            NotifyPropertyChanged("CounterColour");

            Task.Factory.StartNew(() =>
            {
                int round = actualRound;
                while (round == actualRound && 0 < remainingSecondsByRound[round])
                {
                    Thread.Sleep(1000);

                    if (round != actualRound)
                    {
                        break;
                    }

                    remainingSecondsByRound[round]--;
                    NotifyPropertyChanged("RemainingSeconds");

                    if (remainingSecondsByRound[round] <= 3)
                    {
                        CounterColour = lastSecondsColour;
                        NotifyPropertyChanged("CounterColour");

                        counterSoundPlayer.Play();
                    }
                }

                if (round == actualRound)
                {
                    StopCounter();
                }
                remainingSecondsByRound.Remove(round);
            });
        }

        private void StopCounter()
        {
            actualRound++;
            remainingSecondsByRound[actualRound] = 0;
            AvailableFields = new List<DrawableField>();
            IsCounterRunning = false;
            NotifyPropertyChanged("IsCounterRunning");
        }
        #endregion

        public void ChooseColour(string tag)
        {
            StopCounter();
            DrawableField chosenField = map.FirstOrDefault(field => field.Tag == tag);
            int chosenColour = DecodeColur(chosenField.Colour);

            try
            {
                gameServer.ChooseColour(chosenColour);
            }
            catch (Exception)
            {
                MessageBox.Show("Connection lost.", "Hanksite", MessageBoxButton.OK);
                Application.Current.Shutdown();
                return;
            }
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
            RefreshGame(state);
            actualRound++;

            if (availableCells == null)
            {
                return;
            }

            AvailableFields = mapConverter.ConvertToDrawable(availableCells, mapAttributes, CanvasWidth, CanvasHeight);

            StartCounter();

            nextTurnSoundPlayer.Play();
        }

        public void SendGamePlayerSnapshot(List<GamePlayer> players)
        {
            Players = players;
        }

        public void SendGameOver(GameState state)
        {
            RefreshGame(state);
            ClientProxyManager.Instance.RemoveGame();

            GameOverDialog dialog = new GameOverDialog(Window.GetWindow(View), state.Players);
            dialog.ShowDialog();

            NavigationService.GetNavigationService(View).Navigate(new MainMenu());
        }
        #endregion

        public void RefreshGame(GameState state)
        {
            if (mapAttributes == null)
            {
                mapAttributes = mapConverter.GetMapAttributes(state.Map);
            }

            map = mapConverter.ConvertToDrawable(state.Map, CanvasWidth, CanvasHeight);
            NotifyPropertyChanged("Map");

            Players = state.Players;

            RefreshFieldsToStripe(state);
        }

        private void RefreshFieldsToStripe(GameState state)
        {
            long playerID = ClientProxyManager.Instance.UserID;

            List<Coordinate> playerCoordinates = new List<Coordinate>();
            List<Coordinate> enemyCoordinates = new List<Coordinate>();

            foreach (var field in state.Map)
            {
                List<long> playerIDs = state.Players.Select(player => player.ID).ToList();
                if (field.OwnerId == playerID)
                {
                    playerCoordinates.Add(new Coordinate(field.X, field.Y));
                }
                else if (playerIDs.Contains(field.OwnerId))
                {
                    enemyCoordinates.Add(new Coordinate(field.X, field.Y));
                }
            }

            PlayerFields = mapConverter.ConvertToDrawable(playerCoordinates, mapAttributes, CanvasWidth, CanvasHeight);
            EnemyFields = mapConverter.ConvertToDrawable(enemyCoordinates, mapAttributes, CanvasWidth, CanvasHeight);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ColourConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int colourNumber = int.Parse(values[0].ToString());
            return new SolidColorBrush(new ColourProvider().Colours[colourNumber]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PlayerNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            GamePlayer player = (GamePlayer)values[0];
            if (player.Type == Model.PlayerType.BOT)
            {
                return $"{player.Username} [{((BotPlayer)player).difficulty}]";
            }
            else if (player.Type == Model.PlayerType.DISCONNECTED)
            {
                return $"{player.Username} [DISCONNECTED]";
            }
            return player.Username;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
