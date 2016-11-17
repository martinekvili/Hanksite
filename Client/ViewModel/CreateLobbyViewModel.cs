using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using Client.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Linq;
using System.Threading.Tasks;
using Client.ViewModel.Interfaces;
using Common.Lobby;
using System.Threading;

namespace Client.ViewModel
{
    class CreateLobbyViewModel : INotifyPropertyChanged, ILobbyActions
    {
        private const int MAX_PLAYERS = 8;
        private const int MAX_COLOURS = 16;
        private const int MIN_PLAYER_COLOUR_DIFF = 3;

        #region dialogs
        private const string DIALOG_CAPTION = "Hanksite";
        private const MessageBoxButton DIALOG_BUTTON = MessageBoxButton.OK;
        private const MessageBoxImage DIALOG_ICON = MessageBoxImage.Warning;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        public ICommand ReadyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand StartCommand { get; set; }
        public ICommand BackCommand { get; set; }

        #region lobby properties
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;

                if (name != "")
                {
                    IsReadyEnabled = true;
                }
                else
                {
                    IsReadyEnabled = false;
                }

                NotifyPropertyChanged("Name");
            }
        }

        public int[] NumberOfPlayers { get; set; }
        private int selectedNumberOfPlayers;
        public int SelectedNumberOfPlayers
        {
            get { return selectedNumberOfPlayers; }
            set { selectedNumberOfPlayers = value; RefreshNumberOfColours(); RefreshBots(); }
        }
        public ObservableCollection<int> NumberOfColours { get; set; }
        private int selectedNumberOfColours;
        public int SelectedNumberOfColours
        {
            get { return selectedNumberOfColours; }
            set { selectedNumberOfColours = value; NotifyPropertyChanged("SelectedNumberOfColours"); }
        }
        #endregion

        #region bot properties
        private Dictionary<BotDifficulty, int> bots;

        public int NumberOfEasyBots => bots[BotDifficulty.EASY];
        public int NumberOfMediumBots => bots[BotDifficulty.MEDIUM];
        public int NumberOfHardBots => bots[BotDifficulty.HARD];

        public ICommand IncreaseBotCommand { get; set; }
        public ParameterizedCommandHandler DecreaseBotCommand { get; set; }
        #endregion

        #region connected player properties
        private IConnectedPlayerProvider connectedPlayerProvider;
        private List<Player> connectedPlayerAdapter;
        private ObservableCollection<Player> connectedPlayers;
        public ObservableCollection<Player> ConnectedPlayers
        {
            get { return connectedPlayers; }
            set { connectedPlayers = value; NotifyPropertyChanged("ConnectedPlayers"); }
        }
        #endregion

        private bool isReadyEnabled = false;
        public bool IsReadyEnabled
        {
            get { return isReadyEnabled; }
            set { isReadyEnabled = value; NotifyPropertyChanged("IsReadyEnabled"); }
        }

        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady && isCreator; }
            set { isReady = value; NotifyPropertyChanged("IsReady"); NotifyPropertyChanged("IsCanceled"); }
        }
        public bool IsCanceled
        {
            get { return !isReady && isCreator; }
            set { isReady = !value; NotifyPropertyChanged("IsReady"); NotifyPropertyChanged("IsCanceled"); }
        }

        private bool isCreator = true;
        public bool IsJoiner
        {
            get { return !isCreator; }
            set { isCreator = !value; NotifyPropertyChanged("IsCreator"); NotifyPropertyChanged("IsJoiner"); }
        }

        public CreateLobbyViewModel()
        {
            NumberOfColours = new ObservableCollection<int>();
            SetNumberOfColours(2);

            NumberOfPlayers = new int[MAX_PLAYERS - 1];
            for (int i = 0; i < MAX_PLAYERS - 1; i++)
            {
                NumberOfPlayers[i] = i + 2;
            }

            connectedPlayerProvider = new Players();

            ReadyCommand = new CommandHandler(Ready, true);
            CancelCommand = new CommandHandler(Cancel, true);
            StartCommand = new CommandHandler(Start, true);
            BackCommand = new CommandHandler(Back, true);

            bots = new Dictionary<BotDifficulty, int>();
            bots[BotDifficulty.EASY] = 0;
            bots[BotDifficulty.MEDIUM] = 0;
            bots[BotDifficulty.HARD] = 0;

            IncreaseBotCommand = new ParameterizedCommandHandler(IncreaseBot, IsIncreaseBotEnabled);
            DecreaseBotCommand = new ParameterizedCommandHandler(DecreaseBot, IsDecreaseBotEnabled);

            connectedPlayerAdapter = new List<Player>();
            connectedPlayers = new ObservableCollection<Player>();
        }

        public void SetLobby(Lobby lobby)
        {
            IsJoiner = true;
            IsReady = true;

            Name = lobby.Name;
            SelectedNumberOfPlayers = lobby.NumberOfPlayers;
            SelectedNumberOfColours = lobby.NumberOfColours;

            bots = lobby.Bots;
            NotifyNumberOfBotsChanged();

            ConnectedPlayers = new ObservableCollection<Player>(lobby.ConnectedPlayers);
        }

        #region bot methods
        private void IncreaseBot(object parameter)
        {
            BotDifficulty difficulty = (BotDifficulty)parameter;
            bots[difficulty] = bots[difficulty] + 1;

            NotifyNumberOfBotsChanged();
        }

        private void DecreaseBot(object parameter)
        {
            BotDifficulty difficulty = (BotDifficulty)parameter;
            bots[difficulty] = bots[difficulty] - 1;

            NotifyNumberOfBotsChanged();
        }

        private void NotifyNumberOfBotsChanged()
        {
            NotifyPropertyChanged("NumberOfEasyBots");
            NotifyPropertyChanged("NumberOfMediumBots");
            NotifyPropertyChanged("NumberOfHardBots");
        }

        private bool IsIncreaseBotEnabled(object parameter)
        {
            if (1 < SelectedNumberOfPlayers - bots.Sum(bot => bot.Value))
            {
                return true;
            }

            return false;
        }

        private bool IsDecreaseBotEnabled(object parameter)
        {
            BotDifficulty difficulty = (BotDifficulty)parameter;
            if (0 < bots[difficulty])
            {
                return true;
            }

            return false;
        }
        #endregion

        private void Ready()
        {
            IsReady = true;

            SimulatePlayers();
        }

        private void SimulatePlayers()
        {
            Task.Factory.StartNew(() =>
            {
                List<Player> playerContainer = connectedPlayerProvider.GetPlayers();

                Thread.Sleep(1000);
                connectedPlayerAdapter.Add(playerContainer[0]);
                SendLobbyMembersSnapshot(connectedPlayerAdapter);
                Thread.Sleep(500);
                connectedPlayerAdapter.Add(playerContainer[1]);
                SendLobbyMembersSnapshot(connectedPlayerAdapter);
                Thread.Sleep(1000);
                connectedPlayerAdapter.Add(playerContainer[2]);
                SendLobbyMembersSnapshot(connectedPlayerAdapter);
            });
        }

        private void Cancel()
        {
            IsReady = false;
        }

        private void Start()
        {
            NavigationService.GetNavigationService(View).Navigate(new GameView());
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }

        private void SetNumberOfColours(int selectedNumberOfPlayers)
        {
            NumberOfColours.Clear();
            int minNumberOfColours = selectedNumberOfPlayers + MIN_PLAYER_COLOUR_DIFF;
            for (int i = 0; i < MAX_COLOURS - minNumberOfColours; i++)
            {
                NumberOfColours.Add(minNumberOfColours + i + 1);
            }
        }

        private void RefreshNumberOfColours()
        {
            int actualNumberOfColours = NumberOfColours[SelectedNumberOfColours];

            SetNumberOfColours(SelectedNumberOfPlayers);

            if (NumberOfColours.Contains(actualNumberOfColours))
            {
                SelectedNumberOfColours = NumberOfColours.IndexOf(actualNumberOfColours);
            }
            else
            {
                SelectedNumberOfColours = 0;
            }
        }

        private void RefreshBots()
        {
            if (SelectedNumberOfPlayers <= bots.Sum(bot => bot.Value))
            {
                bots[BotDifficulty.EASY] = 0;
                bots[BotDifficulty.MEDIUM] = 0;
                bots[BotDifficulty.HARD] = 0;

                NotifyNumberOfBotsChanged();
            }
        }

        #region callbacks
        public void SendLobbyMembersSnapshot(List<Player> lobbySnapshot)
        {
            ConnectedPlayers = new ObservableCollection<Player>(lobbySnapshot);
        }

        public void SendLobbyClosed()
        {
            string message = "The lobby you connected to has closed.";
            MessageBox.Show(message, DIALOG_CAPTION, DIALOG_BUTTON, DIALOG_ICON);
            Back();
        }

        public void SendNotEnoughPlayers()
        {
            string message = "There are not enough players in the lobby.";
            MessageBox.Show(message, DIALOG_CAPTION, DIALOG_BUTTON, DIALOG_ICON);
        }
        #endregion

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
