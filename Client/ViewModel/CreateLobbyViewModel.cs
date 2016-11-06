using Client.Model;
using Client.Model.Dummy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Client.ViewModel
{
    class CreateLobbyViewModel : INotifyPropertyChanged
    {
        private const int MAX_PLAYERS = 8;
        private const int MAX_COLOURS = 16;
        private const int MIN_PLAYER_COLOUR_DIFF = 3;

        private Dictionary<BotDifficulty, int> bots;
        private IConnectedPlayerProvider connectedPlayerProvider;
        private int selectedNumberOfColours;

        public event PropertyChangedEventHandler PropertyChanged;

        public int[] NumberOfPlayers { get; set; }
        public string SelectedNumberOfPlayers { get; set; }
        public ObservableCollection<int> NumberOfColours { get; set; }
        public int SelectedNumberOfColours { get { return selectedNumberOfColours; } set { selectedNumberOfColours = value; NotifyPropertyChanged("SelectedNumberOfColours"); } }
        public ObservableCollection<string> BotList { get; set; }
        public List<string> BotDifficultyList { get; set; }
        public string SelectedBotDifficulty { get; set; }

        public List<Player> ConnectedPlayers { get { return connectedPlayerProvider.GetPlayers(); } }

        public CreateLobbyViewModel()
        {
            NumberOfPlayers = new int[MAX_PLAYERS - 1];
            for (int i = 0; i < MAX_PLAYERS - 1; i++)
            {
                NumberOfPlayers[i] = i + 2;
            }

            NumberOfColours = new ObservableCollection<int>();
            setNumberOfColours(2);

            BotList = new ObservableCollection<string>();
            bots = new Dictionary<BotDifficulty, int>();
            BotDifficultyList = new List<string>();
            foreach (BotDifficulty difficulty in Enum.GetValues(typeof(BotDifficulty)))
            {
                bots.Add(difficulty, 0);
                BotDifficultyList.Add(difficulty.convertToString());
            }

            connectedPlayerProvider = new Players();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void setNumberOfColours(int selectedNumberOfPlayers)
        {
            NumberOfColours.Clear();
            int minNumberOfColours = selectedNumberOfPlayers + MIN_PLAYER_COLOUR_DIFF;
            for (int i = 0; i < MAX_COLOURS - minNumberOfColours; i++)
            {
                NumberOfColours.Add(minNumberOfColours + i + 1);
            }
        }

        public void refreshNumberOfColours()
        {
            int actualNumberOfColours = NumberOfColours[selectedNumberOfColours];

            if (SelectedNumberOfPlayers != null)
            {
                setNumberOfColours(int.Parse(SelectedNumberOfPlayers));
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

        public void addBot(BotDifficulty difficulty)
        {
            bots[difficulty] = bots[difficulty] + 1;
            refreshBotList();
        }

        public void addSelectedBot()
        {
            if (SelectedBotDifficulty == "Easy")
            {
                addBot(BotDifficulty.EASY);
            }
            else if (SelectedBotDifficulty == "Medium")
            {
                addBot(BotDifficulty.MEDIUM);
            }
            else if (SelectedBotDifficulty == "Hard")
            {
                addBot(BotDifficulty.HARD);
            }
        }

        public void removeBot(string difficultyName)
        {
            BotDifficulty difficulty = BotDifficultyHelper.convertFromString(difficultyName);
            bots[difficulty] = bots[difficulty] - 1;
            refreshBotList();
        }

        private void refreshBotList()
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
    } 
}
