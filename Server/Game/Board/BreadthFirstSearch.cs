using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Board
{
    public static class BreadthFirstSearch
    {
        public static List<Hexagon> Search(Hexagon startCell, Func<Hexagon, bool> isCellMatching, bool perimeterOnly)
        {
            List<Hexagon> matchingCells = new List<Hexagon>();

            HashSet<Hexagon> alreadyVisited = new HashSet<Hexagon>();
            Queue<Hexagon> bfsQueue = new Queue<Hexagon>();
            bfsQueue.Enqueue(startCell);

            while (bfsQueue.Count != 0)
            {
                Hexagon cell = bfsQueue.Dequeue();
                if (alreadyVisited.Contains(cell))
                    continue;

                alreadyVisited.Add(cell);

                if (!isCellMatching(cell))
                {
                    if (perimeterOnly)
                        matchingCells.Add(cell);

                    continue;
                }

                if (!perimeterOnly)
                    matchingCells.Add(cell);

                foreach (var neighbour in cell.Neighbours)
                    bfsQueue.Enqueue(neighbour);
            }

            return matchingCells;
        }
    }
}
