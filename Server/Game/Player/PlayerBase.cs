using Server.Game.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Users;

namespace Server.Game.Player
{
    public abstract class PlayerBase
    {
        protected readonly User user;
        protected readonly GameManager game;

        public User User => user;

        public long ID => user.ID;

        public int Points { get; set; }
        public int Colour { get; set; }
        public int Position { get; set; }

        public PlayerBase(User user, GameManager game)
        {
            this.user = user;
            this.game = game;

            this.Points = 1;
            this.Position = 1;
        }

        public PlayerBase(PlayerBase other)
        {
            this.user = other.user;
            this.game = other.game;
            this.Points = other.Points;
            this.Colour = other.Colour;
            this.Position = other.Position;
        }

        public virtual bool CanDoStep => true;

        public abstract void SendGameSnapshot();
        public abstract void DoNextStep(List<Hexagon> availableCells);
        public abstract void SendGamePlayersSnapshot();
        public abstract void SendGameOver();
        public abstract bool IsReady { get; }
        public abstract Common.Game.Player ToDto();
    }
}
