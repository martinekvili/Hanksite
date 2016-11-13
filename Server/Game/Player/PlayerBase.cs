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

        public int ID => user.ID;

        public int Points { get; set; }
        public int CurrentColour { get; set; }
        public int CurrentPosition { get; set; } = 1;

        public PlayerBase(User user, GameManager game)
        {
            this.user = user;
            this.game = game;

            this.Points = 1;
        }

        public virtual bool CanDoStep => true;

        public abstract void SendGameSnapshot();
        public abstract void DoNextStep(List<Hexagon> availableCells);
        public abstract void SendGameOver();
        public abstract Common.Game.Player ToDto();
    }
}
