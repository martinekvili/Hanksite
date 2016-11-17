using Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServerConnection
{
    public partial class ClientCallback
    {
        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            throw new NotImplementedException();
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public void SendGamePlayerSnapshot(GamePlayersSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}
