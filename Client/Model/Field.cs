using Common.Game;
using System.Windows.Media;

namespace Client.Model
{
    public class Field
    {
        public long OwnerId { get;}
        public int X { get; }
        public int Y { get; }
        public int Colour { get; }

        public Field(int OwnerId, int x, int y, int colour)
        {
            X = x;
            Y = y;
            Colour = colour;
        }

        public Field(Hexagon hexagon)
        {
            OwnerId = hexagon.OwnerID;
            X = hexagon.Coord.X;
            Y = hexagon.Coord.Y;
            Colour = hexagon.Colour;
        }
    }
}
