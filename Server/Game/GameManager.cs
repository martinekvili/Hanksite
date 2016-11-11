using Server.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Lobby;
using Server.Game.Board;
using Server.Utils;


namespace Server.Game
{
    public class GameManager
    {
        private readonly object syncObject = new object();

        private int currentPlayerNum;
        private readonly List<PlayerBase> players;
        private readonly Map map;

        public GameManager(List<HanksiteSession> realPlayers, LobbySettings settings)
        {
            this.players = new List<PlayerBase>();
            initializePlayers(realPlayers, settings);

            this.map = MapBuilder.CreateMap(players.Select(player => player.ID).ToList());

            this.currentPlayerNum = 0;
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
            List<Hexagon> selectableCells = null;
            int nextPlayerCandidate;

            do
            {
                nextPlayerCandidate = (currentPlayerNum + 1) % players.Count;

                if (!players[nextPlayerCandidate].CanDoStep)
                    continue;

                selectableCells = map.GetSelectableCellsForPlayer(players[nextPlayerCandidate].ID).ToList();

            } while ((!players[nextPlayerCandidate].CanDoStep || selectableCells.Count == 0) && nextPlayerCandidate != currentPlayerNum);

            if (nextPlayerCandidate == currentPlayerNum) // there's noone who could move
            {
                gameOver();
                return;
            }

            currentPlayerNum = nextPlayerCandidate;

            GameSnapshot snapshot = new GameSnapshot(players, map);
            for (int i = 0; i < players.Count; i++)
            {
                if (i != currentPlayerNum)
                    players[i].SendGameSnapshot(snapshot);
            }

            players[currentPlayerNum].DoNextStep(new GameSnapshotForNextPlayer(players, map, selectableCells));
        }

        private void gameOver()
        {
            GameSnapshot snapshot = new GameSnapshot(players, map);

            foreach (var player in players)
            {

                player.SendGameOver(snapshot);
            }
        }

        public void ChooseColour(PlayerBase player, int colour)
        {
            lock (syncObject)
            {
                player.Points = map.SetPlayerColour(player.ID, colour);

                stepNextPlayerNoLock();
            }
        }

        public void DisconnectPlayer(PlayerBase player)
        {
            lock (syncObject)
            {
                int playerNum = players.FindIndex(p => p.ID == player.ID);

                if (playerNum == -1)
                    return;

                players[playerNum] = new DisconnectedPlayer(player.ID, this);

                if (playerNum == currentPlayerNum)
                    stepNextPlayerNoLock();
            }
        }

        public bool IsPlayerInGame(int playerId)
        {
            lock (syncObject)
            {
                return players.FindIndex(player => player.ID == playerId) != -1;
            }
        }
    }
}
