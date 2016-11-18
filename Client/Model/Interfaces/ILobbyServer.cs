using Common.Lobby;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface ILobbyServer
    {
        Task<bool> CreateLobby(Lobby settings);
        Task<Lobby> ConnectToLobby(string lobbyName);
        void DisconnectFromLobby();
        void StartGame();
    }
}
