using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Player
{
    public class DisconnectedPlayer : PlayerBase
    {
        public DisconnectedPlayer(int id, GameManager game) : base(id, game)
        { }

        public override bool CanDoStep => false;

        public override void DoNextStep(GameSnapshotForNextPlayer snapshot)
        { }

        public override void SendGameOver(GameSnapshot snapshot)
        { }

        public override void SendGameSnapshot(GameSnapshot snapshot)
        { }
    }
}
