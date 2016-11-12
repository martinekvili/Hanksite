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

namespace Client.ViewModel
{
    class CreateLobbyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        public ICommand AddBotCommand { get; set; }
        public ICommand ReadyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand StartCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand RemoveBotCommand { get; set; }

        private const int MAX_PLAYERS = 8;
        private const int MAX_COLOURS = 16;
        private const int MIN_PLAYER_COLOUR_DIFF = 3;

        private string selectedNumberOfPlayers;
        private int selectedNumberOfColours;
        private Dictionary<BotDifficulty, int> bots;
        private IConnectedPlayerProvider connectedPlayerProvider;

        private string name;
        public string Name
        {
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
            }
        }

        public int[] NumberOfPlayers { get; set; }
        public string SelectedNumberOfPlayers
        {
            get { return selectedNumberOfPlayers; }
            set { selectedNumberOfPlayers = value; RefreshNumberOfColours(); }
        }
        public ObservableCollection<int> NumberOfColours { get; set; }
        public int SelectedNumberOfColours
        {
            get { return selectedNumberOfColours; }
            set { selectedNumberOfColours = value; NotifyPropertyChanged("SelectedNumberOfColours"); }
        }
        public ObservableCollection<string> BotList { get; set; }
        public List<string> BotDifficultyList { get; set; }
        public string SelectedBotDifficulty { get; set; }
        public string BotToRemove { get; set; }

        public List<Player> ConnectedPlayers { get { return connectedPlayerProvider.GetPlayers(); } }
        
        private bool isReadyEnabled = false;
        public bool IsReadyEnabled
        {
            get { return isReadyEnabled; }
            set { isReadyEnabled = value; NotifyPropertyChanged("IsReadyEnabled"); }
        }

        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
            set { isReady = value; NotifyPropertyChanged("IsReady"); NotifyPropertyChanged("IsCanceled"); }
        }
        public bool IsCanceled
        {
            get { return !isReady; }
            set { isReady = !value; NotifyPropertyChanged("IsReady"); NotifyPropertyChanged("IsCanceled"); }
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

            BotList = new ObservableCollection<string>();
            bots = new Dictionary<BotDifficulty, int>();
            BotDifficultyList = new List<string>();
            foreach (BotDifficulty difficulty in Enum.GetValues(typeof(BotDifficulty)))
            {
                if (difficulty != BotDifficulty.INVALID)
                {
                    bots.Add(difficulty, 0);
                    BotDifficultyList.Add(difficulty.convertToString());
                }
            }

            connectedPlayerProvider = new Players();

            AddBotCommand = new CommandHandler(AddSelectedBot, true);
            ReadyCommand = new CommandHandler(Ready, true);
            CancelCommand = new CommandHandler(Cancel, true);
            StartCommand = new CommandHandler(Start, true);
            BackCommand = new CommandHandler(Back, true);
            RemoveBotCommand = new CommandHandler(RemoveBot, true);
        }

        private void AddSelectedBot()
        {
            if (SelectedBotDifficulty == "Easy")
            {
                AddBot(BotDifficulty.EASY);
            }
            else if (SelectedBotDifficulty == "Medium")
            {
                AddBot(BotDifficulty.MEDIUM);
            }
            else if (SelectedBotDifficulty == "Hard")
            {
                AddBot(BotDifficulty.HARD);
            }
        }

        private void AddBot(BotDifficulty difficulty)
        {
            bots[difficulty] = bots[difficulty] + 1;
            RefreshBotList();
        }

        private void Ready()
        {
            IsReady = true;
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

        private void RefreshBotList()
        {
            BotList.Clear();
            foreach (var difficulty in bots)
            {
                for (int i = 0; i < difficulty.Value; i++)
                {
                    BotList.Add(difficulty.Key.convertToString());
                }
            }
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

            if (SelectedNumberOfPlayers != null)
            {
                SetNumberOfColours(int.Parse(SelectedNumberOfPlayers));
            }

            if (NumberOfColours.Contains(actualNumberOfColours))
            {
                SelectedNumberOfColours = NumberOfColours.IndexOf(actualNumberOfColours);
            }
            else
            {
                SelectedNumberOfColours = 0;
            }
        }

        private void RemoveBot()
        {
            BotDifficulty difficulty = BotDifficultyHelper.convertFromString(BotToRemove);
            if (difficulty != BotDifficulty.INVALID)
            {
                bots[difficulty] = bots[difficulty] - 1;
            }
            RefreshBotList();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
