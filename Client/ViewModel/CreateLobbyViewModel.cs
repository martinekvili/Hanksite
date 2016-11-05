using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class CreateLobbyViewModel
    {
        private const int MAX_PLAYERS = 8;
        private const int MAX_COLOURS = 16;
        private const int MIN_PLAYER_COLOUR_DIFF = 3;

        private Dictionary<BotDifficulty, int> bots;

        public int[] NumberOfPlayers { get; set; }
        public List<int> NumberOfColours { get; set; }
        public ObservableCollection<string> BotList { get; set; }
        public List<string> BotDifficultyList { get; set; }
        public string SelectedBotDifficulty { get; set; }
        public List<Player> ConnectedPlayers { get; set; }

        public CreateLobbyViewModel()
        {
            NumberOfPlayers = new int[MAX_PLAYERS - 1];
            for (int i = 0; i < MAX_PLAYERS - 1; i++)
            {
                NumberOfPlayers[i] = i + 2;
            }

            NumberOfColours = new List<int>();
            initNumberOfColours(2);

            BotList = new ObservableCollection<string>();

            bots = new Dictionary<BotDifficulty, int>();
            bots.Add(BotDifficulty.EASY, 0);
            bots.Add(BotDifficulty.MEDIUM, 0);
            bots.Add(BotDifficulty.HARD, 0);

            BotDifficultyList = new List<string>();
            BotDifficultyList.Add("Easy");
            BotDifficultyList.Add("Medium");
            BotDifficultyList.Add("Hard");

            ConnectedPlayers = new List<Player>();
            ConnectedPlayers.Add(new Player() { Username = "kazsu04" });
            ConnectedPlayers.Add(new Player() { Username = "jeno9000" });
            ConnectedPlayers.Add(new Player() { Username = "dominator" });
        }

        private void initNumberOfColours(int selectedNumberOfPlayers)
        {
            NumberOfColours.Clear();
            int minNumberOfColours = selectedNumberOfPlayers + MIN_PLAYER_COLOUR_DIFF;
            for (int i = 0; i < MAX_COLOURS - minNumberOfColours; i++)
            {
                NumberOfColours.Add(minNumberOfColours + i + 1);
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

        public void removeBot(string difficulty)
        {
            if (difficulty == "EASY")
            {
                bots[BotDifficulty.EASY] = bots[BotDifficulty.EASY] - 1;
                Console.WriteLine("sasdff");
            }
            else if (difficulty == "MEDIUM")
            {
                bots[BotDifficulty.MEDIUM] = bots[BotDifficulty.MEDIUM] - 1;
            }
            else if (difficulty == "HARD")
            {
                bots[BotDifficulty.HARD] = bots[BotDifficulty.HARD] - 1;
            }

            refreshBotList();
        }

        private void refreshBotList()
        {
            BotList.Clear();
            foreach (var difficulty in bots)
            {
                for (int i = 0; i < difficulty.Value; i++)
                {
                    BotList.Add(difficulty.Key.ToString());
                }
            }
        }
    }

    class Player
    {
        public string Username { get; set; }
    }

    enum BotDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }
}
