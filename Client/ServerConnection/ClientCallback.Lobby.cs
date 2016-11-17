using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ViewModel.Interfaces;
using Common.Lobby;

namespace Client.ServerConnection
{
    public partial class ClientCallback
    {
        //public ILobbyActions Lobby { get; set; }

        public void SendLobbyClosed()
        {
            throw new NotImplementedException();
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbySnapshot)
        {
            throw new NotImplementedException();
        }

        public void SendNotEnoughPlayers()
        {
            throw new NotImplementedException();
        }
    }
}
