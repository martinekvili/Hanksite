using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Board
{
    public class Map : IEnumerable<Hexagon>
    {
        private readonly List<Hexagon> cells;
        private readonly Dictionary<int, Hexagon> playerBases;

        public int CellCount => cells.Count;

        public Common.Game.Hexagon[] ToDto()
        {
            return cells.Select(cell => cell.ToDto()).ToArray();
        }

        /// <summary>
        /// Do NOT call directly, use <see cref="MapBuilder"/> instead!
        /// </summary>
        /// <param name="cells"></param>
        public Map(List<Hexagon> cells, Dictionary<int, Hexagon> playerBases)
        {
            this.cells = cells;
            this.playerBases = playerBases;
        }

        public IEnumerator<Hexagon> GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cells.GetEnumerator();
        }

        public Dictionary<int, int> GetAcquirableCellCountsForPlayer(int playerId, IEnumerable<int> availableColours)
        {
            Hexagon startCell = playerBases[playerId];

            return availableColours.ToDictionary(
                colour => colour,
                colour =>
                {
                    return BreadthFirstSearch.Search(
                            startCell,
                            cell => cell.OwnerID == playerId || cell.Colour == colour,
                            false)
                        .Count;
                });
        }

        public int SetPlayerColour(int playerId, int colour)
        {
            Hexagon startCell = playerBases[playerId];

            List<Hexagon> newlyOwnedCells = BreadthFirstSearch.Search(
                startCell,
                cell => cell.OwnerID == playerId || cell.Colour == colour,
                false);

            foreach (var cell in newlyOwnedCells)
            {
                cell.OwnerID = playerId;
                cell.Colour = colour;
            }

            return newlyOwnedCells.Count;
        }

        public IEnumerable<Hexagon> GetSelectableCellsForPlayer(int playerId, IEnumerable<int> currentPlayerColours)
        {
            HashSet<int> playerColoursSet = new HashSet<int>(currentPlayerColours);

            Hexagon startCell = playerBases[playerId];

            return BreadthFirstSearch.Search(
                    startCell,
                    cell => cell.OwnerID == playerId,
                    true)
                .Where(cell => !playerColoursSet.Contains(cell.Colour));
        }
    }
}
