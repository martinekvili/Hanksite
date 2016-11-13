using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utils;

namespace Server.Game.Board
{
    public class MapBuilder
    {
        public static Map CreateMap(List<int> playerIds, int numberOfColours)
        {
            var builder = createMapBuilderForPlayers(playerIds);
            builder.fillWithColours(numberOfColours);

            return builder.Map;
        }

        /// <summary>
        /// Do NOT call, only used for tests.
        /// </summary>
        public static Map CreateMap(List<int> playerIds)
        {
            return createMapBuilderForPlayers(playerIds).Map;
        }

        private static MapBuilder createMapBuilderForPlayers(List<int> playerIds)
        {
            if (playerIds.Count < 2)
                throw new ArgumentException("Too few players");

            int sideLength;
            if (playerIds.Count <= 6)
                sideLength = 7;
            else
                sideLength = playerIds.Count + 1;

            MapBuilder mapBuilder = new MapBuilder(sideLength);
            mapBuilder.createMap();
            mapBuilder.distributePlayersOnMap(playerIds);

            return mapBuilder;
        }

        /// <summary>
        /// Do NOT call, only used for tests.
        /// </summary>
        public static Map CreateMap(int sideLength)
        {
            MapBuilder mapBuilder = new MapBuilder(sideLength);
            mapBuilder.createMap();

            return mapBuilder.Map;
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

            MapBuilder mapBuilder = new MapBuilder((mapMatrix.GetLength(0) + 1) / 2, mapMatrix);
            mapBuilder.createMap();

            return mapBuilder.Map;
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
        private readonly Dictionary<int, Hexagon> playerBases;
        private List<Hexagon> map;

        public Map Map => new Map(map, playerBases);

        private MapBuilder(int sideLength, Hexagon[,] mapMatrix)
        {
            this.sideLength = sideLength;
            this.mapMatrix = mapMatrix;
            this.playerBases = new Dictionary<int, Hexagon>();
        }

        private MapBuilder(int sideLength) : this(sideLength, new Hexagon[2 * sideLength - 1, 2 * sideLength - 1])
        {
            fillMapMatrix();
        }

        private List<Hexagon> createMap()
        {
            setNeighbours();
            createMapFromMatrix();

            return map;
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


        private void createMapFromMatrix()
        {
            map = new List<Hexagon>(mapMatrix.GetLength(0) * mapMatrix.GetLength(1));

            for (int i = 0; i < mapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < mapMatrix.GetLength(1); j++)
                {
                    if (mapMatrix[i, j] != null)
                        map.Add(mapMatrix[i, j]);
                }              
            }
        }

        private void distributePlayersOnMap(List<int> playerIds)
        {
            List<Hexagon> outerRim = getSortedOuterRimOfMap();

            for (int i = 0; i < playerIds.Count; i++)
            {
                int playerCellNum = (int)Math.Round((double)(i * outerRim.Count) / playerIds.Count);
                Hexagon playerCell = outerRim[playerCellNum];

                playerBases.Add(playerIds[i], playerCell);

                playerCell.OwnerID = playerIds[i];
                playerCell.Colour = i;
            }
        }

        private List<Hexagon> getSortedOuterRimOfMap()
        {
            Coord center = new Coord(sideLength - 1, sideLength - 1);

            return map
                .Where(cell => Coord.Distance(cell.Coord, center) == sideLength - 1)
                .OrderBy(cell => cell.Coord - center, new OuterRimSortComparer(sideLength))
                .ToList();
        }

        #region Outer rim sort comparer
        private class OuterRimSortComparer : IComparer<Coord>
        {
            private readonly int sideLength;
            private readonly Coord firstInOrder;

            public OuterRimSortComparer(int sideLength)
            {
                this.sideLength = sideLength;
                this.firstInOrder = new Coord(-sideLength + 1, sideLength - 1);
            }

            public int Compare(Coord a, Coord b)
            {
                if (a == b)
                    return 0;

                if (a == firstInOrder)  // a is the one we choose to be the first one in order
                    return -1;
                if (b == firstInOrder)  // b is the one we choose to be the first one in order
                    return 1;

                int aSign = Math.Sign(a.Z);
                int bSign = Math.Sign(b.Z);
                if (aSign != bSign)         // they have different Z signs, this tells the order
                    return aSign - bSign;

                int xDiff = a.X - b.X;
                int yDiff = a.Y - b.Y;

                if (aSign == -1)            // they have the same Z sign, the furter ordering depends on that
                {
                    xDiff *= -1;
                    yDiff *= -1;
                }

                if (xDiff != 0)             // first the difference between the x coords chooses
                    return -xDiff;

                return yDiff;               // then the difference between the y coords
            }
        }
        #endregion

        private void fillWithColours(int numberOfColours)
        {
            foreach (var playerBase in playerBases.Values)
            {
                foreach (var playerBaseNeighbour in playerBase.Neighbours)
                    playerBaseNeighbour.Colour = RandomUtils.NextExcluding(numberOfColours, playerBase.Colour);
            }

            foreach (var cell in map)
            {
                if (cell.Colour == -1)
                    cell.Colour = RandomUtils.Next(numberOfColours);
            }
        }
    }
}
