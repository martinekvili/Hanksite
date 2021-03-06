﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Lobby;
using Common.Users;

namespace Server.Lobby
{
    public interface ILobbyMember
    {
        User User { get; }
        HanksiteSession Session { get; }
        void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbyMembersSnapshot);
        void SendLobbyClosed();
        void DisconnectFromLobby();
        void StartGame();
        void SendNotEnoughPlayers();
        void RemoveFromSession();
    }

    public class LobbyMember : ILobbyMember
    {
        private readonly HanksiteSession session;
        private readonly LobbyManager lobby;

        public User User => session.User;
        public HanksiteSession Session => session;

        public LobbyMember(HanksiteSession session, LobbyManager lobby)
        {
            this.session = session;
            this.lobby = lobby;

            this.session.LobbyMember = this;
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbyMembersSnapshot)
        {
            session.SendLobbyMembersSnapshot(lobbyMembersSnapshot);
        }

        public void SendLobbyClosed()
        {
            session.SendLobbyClosed();
        }

        public void DisconnectFromLobby()
        {
            lobby.DisconnectPlayer(this);
        }

        public void StartGame()
        {
            lobby.StartGame();
        }

        public void SendNotEnoughPlayers()
        {
            session.SendNotEnoughPlayers();
        }

        public void RemoveFromSession()
        {
            session.LobbyMember = null;
        }
    }
}
