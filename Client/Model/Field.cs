using System.Windows.Media;

namespace Client.Model
{
    class Field
    {
        public int X { get; }
        public int Y { get; }
        public int Colour { get; }

        public Field(int x, int y, int colour)
        {
            X = x;
            Y = y;
            Colour = colour;
        }
    }
}
