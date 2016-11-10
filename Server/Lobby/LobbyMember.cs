using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Lobby;
using Common.Users;

namespace Server.Lobby
{
    public class LobbyMember
    {
        private readonly HanksiteSession session;
        private readonly LobbyManager lobby;

        public User User;

        public LobbyMember(HanksiteSession session, LobbyManager lobby)
        {
            this.session = session;
            this.lobby = lobby;
        }

        public void SendLobbyMembers(LobbyMembersSnapshot lobbyMembersSnapshot)
        {
            throw new NotImplementedException();
        }

        public void SendDisconnect()
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromLobby()
        {
            lobby.DisconnectPlayer(this);
        }
    }
}
