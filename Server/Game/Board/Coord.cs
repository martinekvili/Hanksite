using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Board
{
    public struct Coord
    {
        private readonly int x;
        private readonly int y;

        public int X => x;
        public int Y => y;
        public int Z => 0 - x - y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.x + b.x, a.y + b.y);
        }

        public static int Distance(Coord a, Coord b)
        {
            int xDiff = Math.Abs(a.X - b.X);
            int yDiff = Math.Abs(a.Y - b.Y);
            int zDiff = Math.Abs(a.Z - b.Z);

            return Math.Max(xDiff, Math.Max(yDiff, zDiff));
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
            return obj is Coord && this == (Coord)obj;
        }

        public override int GetHashCode()
        {
            return (x << 15) | y;
        }

        #endregion
    }
}
