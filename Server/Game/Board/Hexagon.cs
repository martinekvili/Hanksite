using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Board
{
    public class Hexagon
    {
        private readonly Coord coord;
        private readonly List<Hexagon> neighbours;

        public Coord Coord => coord;
        public List<Hexagon> Neighbours => neighbours;

        public int OwnerID { get; set; } = -1;
        public int Colour { get; set; } = -1;

        public Hexagon(Coord coord)
        {
            this.coord = coord;
            this.neighbours = new List<Hexagon>();
        }

        public Common.Game.Hexagon ToDto()
        {
            return new Common.Game.Hexagon
            {
                Coord = coord.ToDto(),
                OwnerID = OwnerID,
                Colour = Colour
            };
        }

        // Equality operators check the equality of the coords, because we know that they are unique regarding cells in the same map,
        // and checking equality only makes sense for cells in the same map.
        #region Equality operators

        public static bool operator ==(Hexagon a, Hexagon b)
        {
            if ((object)a == null && (object)b == null)
                return true;

            if ((object)a == null || (object)b == null)
                return false;

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
