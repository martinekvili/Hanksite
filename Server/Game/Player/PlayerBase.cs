using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Player
{
    public abstract class PlayerBase
    {
        private readonly int id;
        protected readonly GameManager game;

        public int ID => id;
        public int Points { get; set; }

        public PlayerBase(int id, GameManager game)
        {
            this.id = id;
            this.game = game;

            this.Points = 1;
        }

        public virtual bool CanDoStep => true;

        public abstract void SendGameSnapshot(GameSnapshot snapshot);
        public abstract void DoNextStep(GameSnapshotForNextPlayer snapshot);
        public abstract void SendGameOver(GameSnapshot snapshot);
    }
}
