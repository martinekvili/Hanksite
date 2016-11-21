using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public enum PlayerType
    {
        REAL,
        DISCONNECTED,
        BOT
    }

    public class Player
    {
        public string Username { get; set; }
    }

    public class GamePlayer : Player
    {
        public long ID { get; set; }
        public int Position { get; set; }
        public int Colour { get; set; }
        public int Points { get; set; }
        public PlayerType Type { get; set; }
    }

    public class BotPlayer : GamePlayer
    {
        public BotDifficulty difficulty { get; set; }
    }
}
