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

        [TestMethod]
        public void MapBuilderTest_PlayerDistribution()
        {
            var testRows = getPlayerDistributionTestRows();

            foreach (var testRow in testRows)
                playerDistributionTest(testRow);
        }

        #region Player distribution test rows
        public class PlayerDistributionTestRow
        {
            public List<int> PlayerIDs;
            public string expectedMap;

            public PlayerDistributionTestRow(List<int> playerIds, string expectedMap)
            {
                this.PlayerIDs = playerIds;
                this.expectedMap = expectedMap;
            }
        }

        private static List<PlayerDistributionTestRow> getPlayerDistributionTestRows()
        {
            return new List<PlayerDistributionTestRow>
            {
                 new PlayerDistributionTestRow(
                    new List<int> { 0, 1, 2 },
                    @"
............A
.............
.............
.............
.............
.............
C............
.............
.............
.............
.............
.............
......B......
"
                    ),

                new PlayerDistributionTestRow(
                    new List<int> { 0, 1, 2, 3 },
                    @"
............A
.............
.............
...D.........
.............
.............
.............
.............
.............
.........B...
.............
.............
C............
"
                    ),

                new PlayerDistributionTestRow(
                    new List<int> { 0, 1, 2, 3, 4 },
                    @"
............A
.....E.......
.............
.............
.............
.............
.............
...........B.
D............
.............
.............
.............
....C........
"
                    ),

                new PlayerDistributionTestRow(
                    new List<int> { 0, 1, 2, 3, 4, 5, 6 },
                    @"
........G.....A
...............
...............
...............
...............
..F............
..............B
...............
...............
...............
...............
E..............
.........C.....
...............
...D...........
"
                    )
            };
        }
        #endregion

        public void playerDistributionTest(PlayerDistributionTestRow testRow)
        {
            Map map = MapBuilder.CreateMap(testRow.PlayerIDs);

            string mapString = map.ToMapString(cell =>
                    cell != null && cell.OwnerID != -1 ? (char) (cell.OwnerID + 'A') : '.');
            Assert.AreEqual(testRow.expectedMap, mapString);
        }
    }
}
