using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Map : IEnumerable<Hexagon>
    {
        private readonly List<Hexagon> cells;

        public int CellCount => cells.Count;

        /// <summary>
        /// Do NOT call directly, use <see cref="MapBuilder"/> instead!
        /// </summary>
        /// <param name="cells"></param>
        public Map(List<Hexagon> cells)
        {
            this.cells = cells;
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
            Hexagon startCell = cells.First(cell => cell.OwnerID == playerId);

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
            Hexagon startCell = cells.First(cell => cell.OwnerID == playerId);

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

        public IEnumerable<Hexagon> GetSelectableCellsForPlayer(int playerId)
        {
            Hexagon startCell = cells.First(cell => cell.OwnerID == playerId);

            return BreadthFirstSearch.Search(
                    startCell,
                    cell => cell.OwnerID == playerId,
                    true)
                .Where(cell => cell.OwnerID == -1);
        }
    }
}
