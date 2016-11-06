using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace UnitTests
{
    [TestClass]
    public class MapBuilderTest
    {
        [TestMethod]
        public void MapBuilderTest_TestStructure()
        {
            string expectedMapString = @"
...####
..#####
.######
#######
######.
#####..
####...
";
            int sideLength = 4;

            List<Hexagon> map = MapBuilder.CreateMap(sideLength);

            Assert.AreEqual(expectedMapString, createMapString(sideLength, map));
        }

        private static string createMapString(int sideLength, List<Hexagon> map)
        {
            Hexagon[,] mapMatrix = new Hexagon[sideLength * 2 - 1, sideLength * 2 - 1];
            foreach (var cell in map)
                mapMatrix[cell.Coord.X, cell.Coord.Y] = cell;

            StringBuilder mapStringBuilder = new StringBuilder();
            mapStringBuilder.AppendLine();

            for (int i = 0; i < mapMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < mapMatrix.GetLength(1); j++)
                    mapStringBuilder.Append(mapMatrix[i, j] == null ? "." : "#");

                mapStringBuilder.AppendLine();
            }

            return mapStringBuilder.ToString();
        }

        [TestMethod]
        public void MapBuilderTest_NeighboursTest()
        {
            Dictionary<int, int> expectedNeighbourCounts = new Dictionary<int, int>
            {
                { 3, 6 },   // edges
                { 4, 6 },   // sides
                { 6, 7 }    // inner cells
            };

            List<Hexagon> map = MapBuilder.CreateMap(3);

            Assert.AreEqual(expectedNeighbourCounts.Values.Sum(), map.Count);
            CollectionAssert.AreEquivalent(expectedNeighbourCounts, countNeightbours(expectedNeighbourCounts.Keys, map));
        }

        private static Dictionary<int, int> countNeightbours(IEnumerable<int> counts, List<Hexagon> map)
        {
            return counts.ToDictionary(count => count, count => map.Count(cell => cell.Neighbours.Count == count));
        }
    }
}
