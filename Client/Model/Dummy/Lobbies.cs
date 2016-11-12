using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Dummy
{
    class Lobbies : IAvailableLobbyProvider
    {
        private List<Lobby> lobbies;

        public Lobbies()
        {
            lobbies = new List<Lobby>();

            Dictionary<BotDifficulty, int> bots = new Dictionary<BotDifficulty, int>();
            bots[BotDifficulty.EASY] = 1;
            bots[BotDifficulty.MEDIUM] = 2;

            List<Player> players = new List<Player>();
            players.Add(new Player() { Username = "Kornyek" });
            players.Add(new Player() { Username = "winer" });
            players.Add(new Player() { Username = "Timtirim" });

            lobbies.Add(new Lobby() { Name = "BestLobbyEVER", NumberOfPlayers = 8, NumberOfColours = 16, Bots = bots, ConnectedPlayers = players });
            lobbies.Add(new Lobby() { Name = "1604", NumberOfPlayers = 4, NumberOfColours = 8, Bots = new Dictionary<BotDifficulty, int>(), ConnectedPlayers = players });
            lobbies.Add(new Lobby() { Name = "TheSecondBestLobbyThisAfternoon", NumberOfPlayers = 12, NumberOfColours = 16, Bots = bots, ConnectedPlayers = new List<Player>() });
            lobbies.Add(new Lobby() { Name = "heyheyhey", NumberOfPlayers = 7, NumberOfColours = 13, Bots = bots, ConnectedPlayers = players });
        }

        public List<Lobby> GetLobbies()
        {
            return lobbies;
        }
    }
}
