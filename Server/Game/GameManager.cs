using Server.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Board;


namespace Server.Game
{
    public class GameManager
    {
        private readonly object syncObject = new object();

        private int nextPlayer;
        private readonly List<PlayerBase> players;
        private readonly Map map;

        public GameManager(List<PlayerBase> players)
        {
            this.players = players;
            shufflePlayers();

            this.nextPlayer = 0;
        }

        // Uses the Fisher-Yates shuffle algorithm.
        // See: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        private void shufflePlayers()
        {
            Random random = new Random();

            for (int i = players.Count - 1; i > 1; i--)
            {
                int j = random.Next(i + 1);

                PlayerBase temp = players[i];
                players[i] = players[j];
                players[j] = temp;
            }
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
                nextPlayerCandidate = (nextPlayer + 1) % players.Count;

                if (!players[nextPlayerCandidate].CanDoStep)
                    continue;

                selectableCells = map.GetSelectableCellsForPlayer(players[nextPlayerCandidate].ID).ToList();

            } while ((!players[nextPlayerCandidate].CanDoStep || selectableCells.Count == 0) && nextPlayerCandidate != nextPlayer);

            if (nextPlayerCandidate == nextPlayer) // there's noone who could move
            {
                gameOver();
                return;
            }

            nextPlayer = nextPlayerCandidate;

            GameSnapshot snapshot = new GameSnapshot(players, map);
            for (int i = 0; i < players.Count; i++)
            {
                if (i != nextPlayer)
                    players[i].SendGameSnapshot(snapshot);
            }

            players[nextPlayer].DoNextStep(new GameSnapshotForNextPlayer(players, map, selectableCells));
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
    }
}
