using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel.Interfaces
{
    interface ILobbyActions
    {
        void SendLobbyMembersSnapshot(List<Player> lobbySnapshot);
        
        void SendLobbyClosed();
        
        void SendNotEnoughPlayers();
    }
}
