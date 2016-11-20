using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lobby
{
    [ServiceContract(CallbackContract = typeof(ILobbyServiceCallback))]
    public interface ILobbyService
    {
        [OperationContract(IsOneWay = false)]
        LobbySettingsWithMembersSnapshot[] ListLobbies();

        [OperationContract(IsOneWay = false)]
        bool CreateLobby(LobbySettings settings);

        [OperationContract(IsOneWay = false)]
        LobbySettingsWithMembersSnapshot ConnectToLobby(string lobbyName);

        [OperationContract(IsOneWay = true)]
        void DisconnectFromLobby();

        [OperationContract(IsOneWay = true)]
        void StartGame();
    }

    public interface ILobbyServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbySnapshot);

        [OperationContract(IsOneWay = true)]
        void SendLobbyClosed();

        [OperationContract(IsOneWay = true)]
        void SendNotEnoughPlayers();

        [OperationContract(IsOneWay = true)]
        void SendGameStarted();
    }
}
