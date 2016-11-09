using Server.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Board;
using Server.Utils;


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
            players.Shuffle();

            this.nextPlayer = 0;
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
