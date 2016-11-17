using System;
using System.Collections.Generic;
using System.Linq;
using Common.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.Lobby;
using Common.Lobby;
using Common.Users;

namespace UnitTests
{
    [TestClass]
    public class LobbyManagerTest
    {
        [TestMethod]
        public void LobbyManagerTest_Connect_Simple()
        {
            var userNames = new List<string>() { "user1", "user2", "user3" };
            List<Mock<ILobbyMember>> lobbyMembers;
            var lobbyManager = setupLobbyManager(6, createBotNumbers(1, 1, 1), userNames, out lobbyMembers);

            lobbyManager.ConnectPlayer(null);

            assertLobbyMemberGotSnapshot(lobbyMembers[0], userNames.GetRange(0, 2));

            lobbyManager.ConnectPlayer(null);

            assertLobbyMemberGotSnapshot(lobbyMembers[0], userNames);
            assertLobbyMemberGotSnapshot(lobbyMembers[1], userNames);
        }

        [TestMethod]
        public void LobbyManagerTest_Connect_NoRoom()
        {
            var userNames = new List<string>() { "user1", "user2", "user3" };
            List<Mock<ILobbyMember>> lobbyMembers;
            var lobbyManager = setupLobbyManager(6, createBotNumbers(2, 1, 1), userNames, out lobbyMembers);

            Assert.IsTrue(lobbyManager.ConnectPlayer(null));
            Assert.IsFalse(lobbyManager.ConnectPlayer(null));
        }

        [TestMethod]
        public void LobbyManagerTest_Disconnect_NotOwner()
        {
            var userNames = new List<string>() { "user1", "user2", "user3" };
            List<Mock<ILobbyMember>> lobbyMembers;
            var lobbyManager = setupLobbyManager(6, createBotNumbers(0, 1, 1), userNames, out lobbyMembers);

            lobbyManager.ConnectPlayer(null);
            lobbyManager.ConnectPlayer(null);

            resetAllLobbyMembers(lobbyMembers);

            lobbyManager.DisconnectPlayer(lobbyMembers[1].Object);

            userNames.Remove("user2");
            assertLobbyMemberGotSnapshot(lobbyMembers[0], userNames);
            assertLobbyMemberGotSnapshot(lobbyMembers[2], userNames);
        }

        [TestMethod]
        public void LobbyManagerTest_Disconnect_Owner()
        {
            var userNames = new List<string>() { "user1", "user2", "user3" };
            List<Mock<ILobbyMember>> lobbyMembers;
            var lobbyManager = setupLobbyManager(4, createBotNumbers(0, 1, 0), userNames, out lobbyMembers);

            lobbyManager.ConnectPlayer(null);
            lobbyManager.ConnectPlayer(null);

            resetAllLobbyMembers(lobbyMembers);

            lobbyManager.DisconnectPlayer(lobbyMembers[0].Object);

            lobbyMembers[1].Verify(m => m.SendLobbyClosed(), Times.Once);
            lobbyMembers[2].Verify(m => m.SendLobbyClosed(), Times.Once);
        }

        [TestMethod]
        public void LobbyManagerTest_StartGame_Success()
        {
            var userNames = new List<string>() { "user1", "user2", "user3" };
            List<Mock<ILobbyMember>> lobbyMembers;
            Mock<ILobbyManagerRepositoryDependencies> repositoryDependencies;
            LobbySettings settings;
            var lobbyManager = setupLobbyManager(6, createBotNumbers(1, 1, 1), userNames, out lobbyMembers, out repositoryDependencies, out settings);

            lobbyManager.ConnectPlayer(null);
            lobbyManager.ConnectPlayer(null);

            lobbyManager.StartGame();

            lobbyMembers[0].Verify(m => m.RemoveFromSession(), Times.Once);
            lobbyMembers[1].Verify(m => m.RemoveFromSession(), Times.Once);
            lobbyMembers[2].Verify(m => m.RemoveFromSession(), Times.Once);

            repositoryDependencies.Verify(d => d.RemoveLobbyManager(lobbyManager), Times.Once);
            repositoryDependencies.Verify(d => d.StartGame(It.IsAny<List<HanksiteSession>>(), settings), Times.Once);
        }

        [TestMethod]
        public void LobbyManagerTest_StartGame_NotEnough()
        {
            var userNames = new List<string>() { "user1", "user2", "user3" };
            List<Mock<ILobbyMember>> lobbyMembers;
            Mock<ILobbyManagerRepositoryDependencies> repositoryDependencies;
            LobbySettings settings;
            var lobbyManager = setupLobbyManager(8, createBotNumbers(2, 1, 1), userNames, out lobbyMembers, out repositoryDependencies, out settings);

            lobbyManager.ConnectPlayer(null);
            lobbyManager.ConnectPlayer(null);

            lobbyManager.StartGame();

            lobbyMembers[0].Verify(m => m.SendNotEnoughPlayers(), Times.Once);

            repositoryDependencies.Verify(d => d.RemoveLobbyManager(lobbyManager), Times.Never);
            repositoryDependencies.Verify(d => d.StartGame(It.IsAny<List<HanksiteSession>>(), settings), Times.Never);
        }

        #region Helper methods

        private LobbySettingsBotNumber[] createBotNumbers(int hardCount, int mediumCount, int easyCount)
        {
            return new LobbySettingsBotNumber[]
            {
                new LobbySettingsBotNumber { Difficulty = AIDifficulty.Hard, Number = hardCount },
                new LobbySettingsBotNumber { Difficulty = AIDifficulty.Medium, Number = mediumCount },
                new LobbySettingsBotNumber { Difficulty = AIDifficulty.Easy, Number = easyCount },
            };
        }

        private LobbyManager setupLobbyManager(int numberOfPlayers, LobbySettingsBotNumber[] botSettings,
            IEnumerable<string> userNames, out List<Mock<ILobbyMember>> lobbyMembers)
        {
            Mock<ILobbyManagerRepositoryDependencies> repositoryDependencies;
            LobbySettings lobbySettings;

            return setupLobbyManager(numberOfPlayers, botSettings, userNames, out lobbyMembers,
                out repositoryDependencies, out lobbySettings);
        }

        private LobbyManager setupLobbyManager(int numberOfPlayers, LobbySettingsBotNumber[] botSettings,
            IEnumerable<string> userNames, out List<Mock<ILobbyMember>> lobbyMembers,
            out Mock<ILobbyManagerRepositoryDependencies> repositoryDependencies, out LobbySettings lobbySettings)
        {
            lobbyMembers = getLobbyMembersWithNames(userNames);
            var lobbyMemberFactory = createLobbyMemberFactoryForMembers(lobbyMembers);

            repositoryDependencies = new Mock<ILobbyManagerRepositoryDependencies>();

            lobbySettings = new LobbySettings
            {
                NumberOfPlayers = numberOfPlayers,
                BotNumbers = botSettings
            };

            return new LobbyManager(null, lobbySettings, lobbyMemberFactory.Object, repositoryDependencies.Object);
        }

        private List<Mock<ILobbyMember>> getLobbyMembersWithNames(IEnumerable<string> userNames)
        {
            return userNames.Select(name =>
            {
                var lobbyMember = new Mock<ILobbyMember>();
                lobbyMember.SetupGet(m => m.User).Returns(new User { UserName = name });
                return lobbyMember;
            })
            .ToList();
        }

        private Mock<ILobbyMemberFactory> createLobbyMemberFactoryForMembers(List<Mock<ILobbyMember>> lobbyMembers)
        {
            var lobbyMemberFactory = new Mock<ILobbyMemberFactory>();

            var setupResult = lobbyMemberFactory.SetupSequence(lmf => lmf.CreateLobbyMember(It.IsAny<HanksiteSession>(), It.IsAny<LobbyManager>()));
            foreach (var lobbyMember in lobbyMembers)
            {
                setupResult = setupResult.Returns(lobbyMember.Object);
            }

            return lobbyMemberFactory;
        }

        private void assertLobbyMemberGotSnapshot(Mock<ILobbyMember> lobbyMember, IEnumerable<string> userNames)
        {
            lobbyMember.Verify(
                m => m.SendLobbyMembersSnapshot(
                    It.Is<LobbyMembersSnapshot>(snapshot => doesSnapshotContainUsers(snapshot, userNames))
                ),
                Times.Once);
        }

        private bool doesSnapshotContainUsers(LobbyMembersSnapshot snapshot, IEnumerable<string> userNames)
        {
            return snapshot.LobbyMembers
                .Select(member => member.UserName)
                .OrderBy(name => name)
                .SequenceEqual(userNames.OrderBy(name => name));
        }

        private void resetAllLobbyMembers(List<Mock<ILobbyMember>> lobbyMembers)
        {
            foreach (var lobbyMember in lobbyMembers)
                lobbyMember.ResetCalls();
        }

        #endregion
    }
}
