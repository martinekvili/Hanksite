using System;
using System.Windows.Media;

namespace Client.Model
{
    class DrawableField
    {
        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }
        public Brush Colour { get; }

        public DrawableField(float x, float y, float width, float height, Brush colour)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Colour = colour;
        }
    }
}
