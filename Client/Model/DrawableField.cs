using System;
using System.Windows.Media;

namespace Client.Model
{
    public class DrawableField
    {
        public Coordinate LogicalPosition { get; set; }
        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }
        public Brush Colour { get; }

        public DrawableField(Coordinate position, float x, float y, float width, float height, Brush colour)
        {
            LogicalPosition = position;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Colour = colour;
        }
    }
}
