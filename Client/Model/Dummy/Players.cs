using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Dummy
{
    class Players : IConnectedPlayerProvider
    {
        private List<Player> players;

        public Players()
        {
            players = new List<Player>();
            players.Add(new Player() { Username = "kazsu04" });
            players.Add(new Player() { Username = "jeno9000" });
            players.Add(new Player() { Username = "dominator" });
        }

        public List<Player> GetPlayers()
        {
            return players;
        }
    }
}
