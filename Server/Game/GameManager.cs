using Server.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Game;
using Common.Lobby;
using Server.DAL;
using Server.Game.Board;
using Server.Utils;
using AIPlayer = Server.Game.Player.AIPlayer;
using Hexagon = Server.Game.Board.Hexagon;

namespace Server.Game
{
    public class GameManager
    {
        private static readonly TimeSpan timeOutForPlayerChoice = TimeSpan.FromSeconds(15);

        private static int gameManagerCounter = 0;

        private readonly int id;
        private readonly DateTime startTime;
        private readonly string name;
        private Timer playerTimeoutTimer;
        private readonly object syncObject = new object();

        private bool isGameOver = false;

        private int currentPlayerNum;
        private readonly List<PlayerBase> players;
        private readonly Map map;

        public int ID => id;

        public Map Map => map;

        public GameSnapshot Snapshot => new GameSnapshot
        {
            Name = name,
            Map = map.ToDto(),
            Players = players.Select(player => player.ToDto()).ToArray()
        };

        public GameSnapshotForDisconnected SnapshotForDisconnected => new GameSnapshotForDisconnected
        {
            ID = id,
            Name = name,
            Players = players.Select(player => player.User).ToArray()
        };

        public GameManager(List<HanksiteSession> realPlayers, LobbySettings settings)
        {
            this.id = Interlocked.Increment(ref gameManagerCounter);
            this.startTime = DateTime.Now;
            this.name = settings.Name;

            this.players = new List<PlayerBase>();
            initializePlayers(realPlayers, settings);

            this.map = MapBuilder.CreateMap(players.Select(player => player.ID).ToList(), settings.NumberOfColours);

            this.currentPlayerNum = -1;
            stepNextPlayer();
        }

        private void initializePlayers(List<HanksiteSession> realPlayers, LobbySettings settings)
        {
            players.AddRange(realPlayers.Select(realPlayer => new RealPlayer(realPlayer, this)));

            int aiPlayerCount = 0;
            foreach (var botNumber in settings.BotNumbers)
            {
                for (int i = 0; i < botNumber.Number; i++)
                {
                    players.Add(new AIPlayer(aiPlayerCount, botNumber.Difficulty, this));
                    aiPlayerCount++;
                }             
            }

            players.Shuffle();

            for (int i = 0; i < players.Count; i++)
                players[i].Colour = i;
        }

        private void stepNextPlayer()
        {
            lock (syncObject)
            {
                stepNextPlayerNoLock();
            }
        }

        private void stepNextPlayerNoLock()
        {
            playerTimeoutTimer?.Dispose();
            playerTimeoutTimer = null;

            List<Hexagon> selectableCells;
            List<int> skippedPlayers;
            if (!getNextPlayerCandidate(out selectableCells, out skippedPlayers))
                return;

            foreach (var playerNum in skippedPlayers)
                players[playerNum].DoNextStep(null);

            sendToAllPlayers(player => player.SendGameSnapshot(), players[currentPlayerNum]);
            players[currentPlayerNum].DoNextStep(selectableCells);

            if (players[currentPlayerNum] is RealPlayer)
                playerTimeoutTimer = new Timer(playerTimeoutCallback, currentPlayerNum, timeOutForPlayerChoice, Timeout.InfiniteTimeSpan);
        }

        private void playerTimeoutCallback(object playerNumObj)
        {
            lock (syncObject)
            {
                int playerNum = (int)playerNumObj;

                if (currentPlayerNum != playerNum)
                    return;

                players[currentPlayerNum].SendTimedOut();
                stepNextPlayerNoLock();
            }
        }

        private bool getNextPlayerCandidate(out List<Hexagon> selectableCells, out List<int> skippedPlayers)
        {
            int nextPlayerCandidate = currentPlayerNum;
            selectableCells = null;
            skippedPlayers = new List<int>();

            do
            {
                nextPlayerCandidate = (nextPlayerCandidate + 1) % players.Count;

                if (!players[nextPlayerCandidate].CanDoStep)
                    continue;

                selectableCells = map.GetSelectableCellsForPlayer(players[nextPlayerCandidate].ID, players.Select(player => player.Colour)).ToList();

                if (selectableCells.Count == 0)
                    skippedPlayers.Add(nextPlayerCandidate);

            } while ((!players[nextPlayerCandidate].CanDoStep || selectableCells.Count == 0) && nextPlayerCandidate != currentPlayerNum);

            if (nextPlayerCandidate == currentPlayerNum) // there's noone who could move
            {
                gameOver();
                return false;
            }

            currentPlayerNum = nextPlayerCandidate;
            return true;
        }

        private void gameOver()
        {
            isGameOver = true;

            GameManagerPool.Instance.RemoveGame(this);

            GameDAL.StoreGame(Snapshot, startTime);

            sendToAllPlayers(player => player.SendGameOver());
        }

        public void ChooseColour(PlayerBase player, int colour)
        {
            lock (syncObject)
            {
                player.Colour = colour;
                player.Points = map.SetPlayerColour(player.ID, colour);
                calculatePositions();

                stepNextPlayerNoLock();
            }
        }

        private void calculatePositions()
        {
            int lastPosition = 1;
            int lastPoints = players.Max(player => player.Points);

            foreach (var player in players.OrderByDescending(player => player.Points))
            {
                if (player.Points != lastPoints)
                {
                    lastPosition++;
                    lastPoints = player.Points;
                }

                player.Position = lastPosition;
            }
        }

        public void DisconnectPlayer(RealPlayer player)
        {
            lock (syncObject)
            {
                int playerNum = players.FindIndex(p => p.ID == player.ID);

                if (playerNum == -1)
                    return;

                players[playerNum] = new DisconnectedPlayer(player);

                if (playerNum == currentPlayerNum)
                {
                    stepNextPlayerNoLock();
                }
                else
                {
                    sendToAllPlayers(p => p.SendGameSnapshot());
                }    
            }
        }

        public bool IsPlayerInGame(long playerId)
        {
            lock (syncObject)
            {
                return players.FindIndex(player => player.ID == playerId) != -1;
            }
        }

        public GameSnapshot ReconnectToGame(HanksiteSession hanksiteSession)
        {
            lock (syncObject)
            {
                if (isGameOver)
                    return null;

                int playerNum = players.FindIndex(player => player.ID == hanksiteSession.User.ID);

                if (playerNum == -1)
                    return null;

                players[playerNum] = new RealPlayer(hanksiteSession, players[playerNum] as DisconnectedPlayer);
                sendToAllPlayers(player => player.SendGameSnapshot(), players[playerNum]);

                return Snapshot;
            }
        }

        private void sendToAllPlayers(Action<PlayerBase> message, PlayerBase excludedPlayer = null)
        {
            foreach (var player in players)
            {
                if (player != excludedPlayer)
                    message(player);
            }
        }
    }
}
