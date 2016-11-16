using Common.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Lobby;

namespace Server
{
    public partial class HanksiteSession
    {
        public LobbyMember LobbyMember { get; set; }

        public LobbySettingsWithMembersSnapshot[] ListLobbies()
        {
            return LobbyManagerRepository.Instance.ListLobbies().ToArray();
        }

        public bool CreateLobby(LobbySettings settings)
        {
            return LobbyManagerRepository.Instance.CreateLobby(this, settings);
        }

        public LobbySettingsWithMembersSnapshot ConnectToLobby(string lobbyName)
        {
            return LobbyManagerRepository.Instance.ConnectToLobby(this, lobbyName);
        }

        public void DisconnectFromLobby()
        {
            LobbyMember.DisconnectFromLobby();

            LobbyMember = null;
        }

        public void StartGame()
        {
            LobbyMember.StartGame();
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbyMembersSnapshot)
        {
            callback.SendLobbyMembersSnapshot(lobbyMembersSnapshot);
        }

        public void SendNotEnoughPlayers()
        {
            callback.SendNotEnoughPlayers();
        }

        public void SendLobbyClosed()
        {
            callback.SendLobbyClosed();

            LobbyMember = null;
        }
    }
}
