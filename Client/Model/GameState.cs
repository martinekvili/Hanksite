using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class GameState
    {
        public List<Field> Map { get; set; }
        public List<GamePlayer> Players { get; set; }
    }

    public class GameStateForDisconnected
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
    }
}
