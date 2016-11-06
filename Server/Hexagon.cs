using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public struct Coord
    {
        private readonly int x;
        private readonly int y;

        public int X => x;
        public int Y => y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.x + b.x, a.y + b.y);
        }

        #region Equality operators

        public static bool operator ==(Coord a, Coord b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Coord && this == (Coord) obj;
        }

        public override int GetHashCode()
        {
            return (x << 15) | y;
        }

        #endregion
    }

    public class Hexagon
    {
        private readonly Coord coord;
        private readonly List<Hexagon> neighbours;

        public Coord Coord => coord;
        public List<Hexagon> Neighbours => neighbours;

        public Hexagon(Coord coord)
        {
            this.coord = coord;
            this.neighbours = new List<Hexagon>();
        }

        // Equality operators check the equality of the coords, because we know that they are unique regarding cells in the same map,
        // and checking equality only makes sense for cells in the same map.
        #region Equality operators

        public static bool operator ==(Hexagon a, Hexagon b)
        {
            return a.coord == b.coord;
        }

        public static bool operator !=(Hexagon a, Hexagon b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Hexagon && this == (Hexagon)obj;
        }

        public override int GetHashCode()
        {
            return coord.GetHashCode();
        }

        #endregion
    }
}
