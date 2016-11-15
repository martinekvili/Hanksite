using System.Collections.Generic;

namespace Client.Model
{
    class Lobby
    {
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfColours { get; set; }
        public Dictionary<BotDifficulty, int> Bots { get; set; }
        public List<Player> ConnectedPlayers { get; set; }
    }
}
