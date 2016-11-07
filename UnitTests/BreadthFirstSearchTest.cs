using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.Board;

namespace UnitTests
{
    [TestClass]
    public class BreadthFirstSearchTest
    {
        private Func<Hexagon, char> toCharFunc;

        private string expectedMap;
        private Map map;
        private List<Hexagon> matchingCells;

        public BreadthFirstSearchTest()
        {
            this.toCharFunc = cell => cell != null && cell.Colour == 2 ? '#' : '.';
        }

        [TestMethod]
        public void BreadthFirstSearchTest_WholeArea()
        {
            string initialMap = @"
...xxxx
..aaaxx
.xxxaxx
xxaaaxx
xaaxxx.
aaaxx..
aaxx...
";
            expectedMap = @"
.......
..###..
....#..
..###..
.##....
###....
##.....
";

            map = MapTestHelper.FromMapString(initialMap);

            matchingCells = BreadthFirstSearch.Search(
                map.Single(cell => cell.Coord == new Coord(6, 0)),
                cell => cell.Colour == 0,
                false);

            assertMatchingCells();
        }

        [TestMethod]
        public void BreadthFirstSearchTest_PerimeterOnly()
        {
            string initialMap = @"
...xxxx
..xxaxx
.xxaaxx
xxxaxxx
xxaxxx.
aaxxx..
xxxx...
";
            expectedMap = @"
....##.
...#.#.
..#..#.
..#.#..
##.#...
..#....
##.....
";

            map = MapTestHelper.FromMapString(initialMap);

            matchingCells = BreadthFirstSearch.Search(
                map.Single(cell => cell.Coord == new Coord(1, 4)),
                cell => cell.Colour == 0,
                true);

            assertMatchingCells();
        }

        private void assertMatchingCells()
        {
            foreach (var cell in matchingCells)
                cell.Colour = 2;

            Assert.AreEqual(expectedMap, map.ToMapString(toCharFunc));
        }
    }
}
