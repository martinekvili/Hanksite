using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Board
{
    public class MapBuilder
    {
        public static Map CreateMap(int sideLength)
        {
            return new Map(new MapBuilder(sideLength).createMap());
        }

        /// <summary>
        /// Do NOT call, only used for tests.
        /// </summary>
        public static Map CreateMap(Hexagon[,] mapMatrix)
        {
            if (mapMatrix.GetLength(0) != mapMatrix.GetLength(1))
                throw new ArgumentException("Map matrix must be a square matrix");

            if (mapMatrix.GetLength(0) % 2 == 0)
                throw new ArgumentException("Row and coloumn count of the map matrix must be an odd number");

            return new Map(new MapBuilder((mapMatrix.GetLength(0) + 1) / 2, mapMatrix).createMap());
        }

        private readonly List<Coord> neighbourCoords = new List<Coord>
        {
            new Coord(0, 1),    // right
            new Coord(-1, 1),   // down, right
            new Coord(-1, 0),   // down, left
            new Coord(0, -1),   // left
            new Coord(1, -1),   // up, left
            new Coord(1, 0)     // up, right
        };

        private readonly int sideLength;
        private readonly Hexagon[,] mapMatrix;

        private MapBuilder(int sideLength, Hexagon[,] mapMatrix)
        {
            this.sideLength = sideLength;
            this.mapMatrix = mapMatrix;
        }

        private MapBuilder(int sideLength) : this(sideLength, new Hexagon[2 * sideLength - 1, 2 * sideLength - 1])
        {
            fillMapMatrix();
        }

        private List<Hexagon> createMap()
        {
            setNeighbours();
            return createMapFromMatrix();
        }

        private void fillMapMatrix()
        {
            for (int i = 0; i < mapMatrix.GetLength(0); i++)
            {
                int rowStart = Math.Max(0, sideLength - i - 1);
                int rowEnd = Math.Min(mapMatrix.GetLength(1), 3 * sideLength - i - 2);

                for (int j = rowStart; j < rowEnd; j++)
                    mapMatrix[i, j] = new Hexagon(new Coord(i, j));
            }
        }

        private void setNeighbours()
        {
            for (int i = 0; i < mapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < mapMatrix.GetLength(1); j++)
                {
                    if (mapMatrix[i, j] == null)
                        continue;

                    mapMatrix[i, j].Neighbours.AddRange(getNeighbouringCells(new Coord(i, j)));
                }
            }
        }

        private IEnumerable<Hexagon> getNeighbouringCells(Coord cellCoord)
        {
            return neighbourCoords.Select(coord =>
            {
                Coord neighbourCoord = cellCoord + coord;
                if (!isCoordOnMap(neighbourCoord))
                    return null;

                return mapMatrix[neighbourCoord.X, neighbourCoord.Y];
            }).Where(cell => cell != null);
        }

        private bool isCoordOnMap(Coord coord)
        {
            return coord.X >= 0 && coord.X < mapMatrix.GetLength(0) && coord.Y >= 0 && coord.Y < mapMatrix.GetLength(1);
        }


        private List<Hexagon> createMapFromMatrix()
        {
            List<Hexagon> map = new List<Hexagon>(mapMatrix.GetLength(0) * mapMatrix.GetLength(1));

            for (int i = 0; i < mapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < mapMatrix.GetLength(1); j++)
                {
                    if (mapMatrix[i, j] != null)
                        map.Add(mapMatrix[i, j]);
                }              
            }

            return map;
        }
    }
}
