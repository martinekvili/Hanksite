using System;
using System.Collections.Generic;
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
        public List<string> Bots { get; set; }
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

            bots = new Dictionary<BotDifficulty, int>();
            addBot(BotDifficulty.HARD);
            addBot(BotDifficulty.EASY);
            addBot(BotDifficulty.MEDIUM);
            addBot(BotDifficulty.EASY);

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
            //bots[difficulty]++;
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
