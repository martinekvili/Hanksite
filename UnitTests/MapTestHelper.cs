using Server.Game.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class MapTestHelper
    {
        public static Map FromMapString(string mapString)
        {
            string[] rows = mapString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length != rows[0].Length)
                throw new ArgumentException("Map matrix must be a square matrix");

            if (rows.Length % 2 == 0)
                throw new ArgumentException("Row and coloumn count of the map matrix must be an odd number");

            Hexagon[,] mapMatrix = new Hexagon[rows.Length, rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    if (rows[i][j] != '.')
                    {
                        Hexagon cell = new Hexagon(new Coord(i, j));
                        cell.Colour = rows[i][j] - 'a';

                        mapMatrix[i, j] = cell;
                    }
                        
                }
            }

            return MapBuilder.CreateMap(mapMatrix);
        }

        public static string ToMapString(this Map map, Func<Hexagon, char> toCharFunc)
        {
            Hexagon[,] mapMatrix = new Hexagon[map.Max(cell => cell.Coord.X) + 1, map.Max(cell => cell.Coord.Y) + 1];
            foreach (var cell in map)
                mapMatrix[cell.Coord.X, cell.Coord.Y] = cell;

            StringBuilder mapStringBuilder = new StringBuilder();
            mapStringBuilder.AppendLine();

            for (int i = 0; i < mapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < mapMatrix.GetLength(1); j++)
                    mapStringBuilder.Append(toCharFunc(mapMatrix[i, j]));

                mapStringBuilder.AppendLine();
            }

            return mapStringBuilder.ToString();
        }

        public static Dictionary<int, int> CountNeighboursByKeys(this Map map, IEnumerable<int> countKeys)
        {
            return countKeys.ToDictionary(key => key, key => map.Count(cell => cell.Neighbours.Count == key));
        }
    }
}
