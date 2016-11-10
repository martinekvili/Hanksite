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

        public bool CreateNewLobby(HanksiteSession ownerSession, LobbySettings settings)
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
    }
}
