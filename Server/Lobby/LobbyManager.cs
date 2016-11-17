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
    #region Dependencies
    public interface ILobbyMemberFactory
    {
        ILobbyMember CreateLobbyMember(HanksiteSession session, LobbyManager lobbyManager);
    }

    public interface ILobbyManagerRepositoryDependencies
    {
        void RemoveLobbyManager(LobbyManager lobbyManager);

        void StartGame(List<HanksiteSession> players, LobbySettings settings);
    }
    #endregion

    #region Dependency implementations
    public class LobbyMemberFactory : ILobbyMemberFactory
    {
        public ILobbyMember CreateLobbyMember(HanksiteSession session, LobbyManager lobbyManager)
        {
            return new LobbyMember(session, lobbyManager);
        }
    }

    public class LobbyManagerRepositoryDependecies : ILobbyManagerRepositoryDependencies
    {
        public void RemoveLobbyManager(LobbyManager lobbyManager)
        {
            LobbyManagerRepository.Instance.RemoveLobbyManager(lobbyManager);
        }

        public void StartGame(List<HanksiteSession> players, LobbySettings settings)
        {
            GameManagerRepository.Instance.CreateGame(players, settings);
        }
    }
    #endregion

    public class LobbyManager
    {
        private readonly object syncObject = new object();

        private readonly ILobbyMemberFactory lobbyMemberFactory;
        private readonly ILobbyManagerRepositoryDependencies repositoryDependencies;

        private readonly LobbySettings settings;
        private readonly int numberOfPlayers;

        private readonly ILobbyMember owner;
        private readonly List<ILobbyMember> connectedPlayers;

        public string Name => settings.Name;

        public static LobbyManager CreateLobbyManager(HanksiteSession ownerSession, LobbySettings settings)
        {
            return new LobbyManager(ownerSession, settings, new LobbyMemberFactory(), new LobbyManagerRepositoryDependecies());
        }

        public LobbySettingsWithMembersSnapshot Snapshot => new LobbySettingsWithMembersSnapshot
        {
            Name = settings.Name,
            NumberOfPlayers = settings.NumberOfPlayers,
            NumberOfColours = settings.NumberOfColours,
            BotNumbers = settings.BotNumbers,
            LobbyMembers = connectedPlayers.Select(player => player.User).ToArray()
        };

        /// <summary>
        /// Do NOT use, only for tests.
        /// </summary>
        public LobbyManager(HanksiteSession ownerSession, LobbySettings settings,
            ILobbyMemberFactory lobbyMemberFactory, ILobbyManagerRepositoryDependencies repositoryDependencies)
        {
            this.lobbyMemberFactory = lobbyMemberFactory;
            this.repositoryDependencies = repositoryDependencies;

            this.owner = lobbyMemberFactory.CreateLobbyMember(ownerSession, this);
            this.settings = settings;
            this.numberOfPlayers = settings.NumberOfPlayers - settings.BotNumbers.Sum(botNumber => botNumber.Number);

            this.connectedPlayers = new List<ILobbyMember>();
            this.connectedPlayers.Add(owner);
        }

        public bool ConnectPlayer(HanksiteSession playerSession)
        {
            lock (syncObject)
            {
                if (connectedPlayers.Count == numberOfPlayers)
                    return false;

                ILobbyMember newMember = lobbyMemberFactory.CreateLobbyMember(playerSession, this);
                connectedPlayers.Add(newMember);

                sendMembersSnapshot(newMember);
                return true;
            }
        }

        public void DisconnectPlayer(ILobbyMember member)
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
                    LobbyManagerRepository.Instance.RemoveLobbyManager(this);
                }
            }
        }

        private void sendMembersSnapshot(ILobbyMember excludedMember = null)
        {
            var LobbyMembersSnapshot = new LobbyMembersSnapshot
            {
                LobbyMembers = connectedPlayers.Select(player => player.User).ToArray()
            };

            sendMessage(player => player.SendLobbyMembersSnapshot(LobbyMembersSnapshot), excludedMember);
        }

        private void sendMessage(Action<ILobbyMember> send, ILobbyMember excludedMember)
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
                    player.RemoveFromSession();

                repositoryDependencies.RemoveLobbyManager(this);
                repositoryDependencies.StartGame(connectedPlayers.Select(player => player.Session).ToList(), settings);
            }
        }
    }
}
