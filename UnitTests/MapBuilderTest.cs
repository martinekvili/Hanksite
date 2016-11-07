using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.Board;

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

            Map map = MapBuilder.CreateMap(sideLength);

            Assert.AreEqual(expectedMapString, map.ToMapString(cell => cell == null ? '.' : '#'));
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

            Map map = MapBuilder.CreateMap(3);

            Assert.AreEqual(expectedNeighbourCounts.Values.Sum(), map.CellCount);
            CollectionAssert.AreEquivalent(expectedNeighbourCounts, map.CountNeighboursByKeys(expectedNeighbourCounts.Keys));
        }
    }
}
