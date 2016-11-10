using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Lobby;

namespace Server.Lobby
{
    public class LobbyManagerPool
    {
        private static readonly LobbyManagerPool instance = new LobbyManagerPool();

        public static LobbyManagerPool Instance => instance;

        private readonly object syncObject = new object();

        private readonly List<LobbyManager> lobbyManagers;

        private LobbyManagerPool()
        {
            lobbyManagers = new List<LobbyManager>();
        }

        public bool CreateLobby(HanksiteSession ownerSession, LobbySettings settings)
        {
            lock (syncObject)
            {
                if (lobbyManagers.Any(manager => manager.Name == settings.Name))
                    return false;

                lobbyManagers.Add(new LobbyManager(ownerSession, settings));
                return true;
            }
        }

        public List<LobbySettingsWithMembersSnapshot> ListLobbies()
        {
            lock (syncObject)
            {
                return lobbyManagers.Select(manager => manager.Snapshot).ToList();
            }
        }

        public LobbySettingsWithMembersSnapshot ConnectToLobby(HanksiteSession playerSession, string lobbyName)
        {
            lock (syncObject)
            {
                LobbyManager lobbyManager = lobbyManagers.SingleOrDefault(lobby => lobby.Name == lobbyName);
                if (lobbyManager == null)
                    return null;

                if (!lobbyManager.ConnectPlayer(playerSession))
                    return null;

                return lobbyManager.Snapshot;
            }
        }

        public void RemoveLobbyManager(LobbyManager lobbyManager)
        {
            lock (syncObject)
            {
                lobbyManagers.Remove(lobbyManager);
            }
        }
    }
}
