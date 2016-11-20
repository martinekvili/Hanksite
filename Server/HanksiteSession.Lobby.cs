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
            log("listed lobbies");
            return LobbyManagerRepository.Instance.ListLobbies().ToArray();
        }

        public bool CreateLobby(LobbySettings settings)
        {
            log($"created lobby with name '{settings.Name}'");
            return LobbyManagerRepository.Instance.CreateLobby(this, settings);
        }

        public LobbySettingsWithMembersSnapshot ConnectToLobby(string lobbyName)
        {
            log($"tries to connect to lobby '{lobbyName}'");
            return LobbyManagerRepository.Instance.ConnectToLobby(this, lobbyName);
        }

        public void DisconnectFromLobby()
        {
            log("disconnects from lobby");
            LobbyMember.DisconnectFromLobby();

            LobbyMember = null;
        }

        public void StartGame()
        {
            log("starts game");
            LobbyMember.StartGame();
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbyMembersSnapshot)
        {
            try
            {
                log("is sent LobbyMembersSnapshot");
                callback.SendLobbyMembersSnapshot(lobbyMembersSnapshot);
            }
            catch (Exception ex)
            {
                logError("sending LobbyMembersSnapshot", ex);
            }
            
        }

        public void SendNotEnoughPlayers()
        {
            try
            {
                log("is sent NotEnoughPlayers");
                callback.SendNotEnoughPlayers();
            }
            catch (Exception ex)
            {
                logError("sending NotEnoughPlayers", ex);
            }
            
        }

        public void SendGameStarted()
        {
            try
            {
                log("is sent GameStarted");
                callback.SendGameStarted();
            }
            catch (Exception ex)
            {
                logError("sending GameStarted", ex);
            }
        }

        public void SendLobbyClosed()
        {
            try
            {
                log("is sent LobbyClosed");
                callback.SendLobbyClosed();
            }
            catch (Exception ex)
            {
                logError("sending LobbyClosed", ex);
            }

            LobbyMember = null;
        }
    }
}
