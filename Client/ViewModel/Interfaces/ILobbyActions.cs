using Client.Model;
using System.Collections.Generic;

namespace Client.ViewModel.Interfaces
{
    interface ILobbyActions
    {
        void SendLobbyMembersSnapshot(List<Player> lobbySnapshot);
        
        void SendLobbyClosed();
        
        void SendNotEnoughPlayers();
    }
}
