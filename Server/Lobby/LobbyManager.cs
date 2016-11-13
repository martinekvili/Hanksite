using Common.Lobby;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game;

namespace Server.Lobby
{
    public class LobbyManager
    {
        private readonly object syncObject = new object();

        private readonly LobbySettings settings;
        private readonly int numberOfPlayers;

        private readonly LobbyMember owner;
        private readonly List<LobbyMember> connectedPlayers;

        public string Name => settings.Name;

        public LobbySettingsWithMembersSnapshot Snapshot => new LobbySettingsWithMembersSnapshot
        {
            Name = settings.Name,
            NumberOfPlayers = settings.NumberOfPlayers,
            NumberOfColours = settings.NumberOfColours,
            BotNumbers = settings.BotNumbers,
            LobbyMembers = connectedPlayers.Select(player => player.User).ToArray()
        };

        public LobbyManager(HanksiteSession ownerSession, LobbySettings settings)
        {
            this.owner = new LobbyMember(ownerSession, this);
            this.settings = settings;
            this.numberOfPlayers = settings.NumberOfPlayers - settings.BotNumbers.Sum(botNumber => botNumber.Number);

            this.connectedPlayers = new List<LobbyMember>();
            this.connectedPlayers.Add(owner);
        }

        public bool ConnectPlayer(HanksiteSession playerSession)
        {
            lock (syncObject)
            {
                if (connectedPlayers.Count == numberOfPlayers)
                    return false;

                LobbyMember newMember = new LobbyMember(playerSession, this);
                connectedPlayers.Add(newMember);

                sendMembersSnapshot(newMember);
                return true;
            }
        }

        public void DisconnectPlayer(LobbyMember member)
        {
            lock (syncObject)
            {
                if (member != owner)
                {
                    connectedPlayers.Remove(member);
                    sendMembersSnapshot();
                }
                else
                {
                    sendMessage(player => player.SendLobbyClosed(), member);
                    LobbyManagerPool.Instance.RemoveLobbyManager(this);
                }
            }
        }

        private void sendMembersSnapshot(LobbyMember excludedMember = null)
        {
            var lobbyMembersSnapshot = new LobbyMembersSnapshot
            {
                LobbyMembers = connectedPlayers.Select(player => player.User).ToArray()
            };

            sendMessage(player => player.SendLobbyMembersSnapshot(lobbyMembersSnapshot), excludedMember);
        }

        private void sendMessage(Action<LobbyMember> send, LobbyMember excludedMember)
        {
            foreach (var connectedPlayer in connectedPlayers)
            {
                if (connectedPlayer != excludedMember)
                    send(connectedPlayer);
            }
        }

        public void StartGame()
        {
            lock (syncObject)
            {
                if (connectedPlayers.Count != numberOfPlayers)
                {
                    owner.SendNotEnoughPlayers();
                    return;
                }

                foreach (var player in connectedPlayers)
                    player.Session.LobbyMember = null;

                LobbyManagerPool.Instance.RemoveLobbyManager(this);

                GameManagerPool.Instance.CreateGame(connectedPlayers.Select(player => player.Session).ToList(), settings);
            }
        }
    }
}
