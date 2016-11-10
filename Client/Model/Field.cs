using System.Windows.Media;

namespace Client.Model
{
    class Field
    {
        public int X { get; }
        public int Y { get; }
        public Brush Colour { get; }

        public Field(int x, int y, Brush colour)
        {
            X = x;
            Y = y;
            Colour = colour;
        }
    }
}
