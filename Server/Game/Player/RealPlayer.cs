using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Player
{
    public class RealPlayer : PlayerBase
    {
        private readonly HanksiteSession session;

        public RealPlayer(HanksiteSession session, GameManager game) : base(session.User.ID, game)
        {
            this.session = session;

            session.RealPlayer = this;
        }

        public override void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            throw new NotImplementedException();
        }

        public override void SendGameOver(GameSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public override void SendGameSnapshot(GameSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}
