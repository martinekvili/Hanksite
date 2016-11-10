using System;
using System.Collections.Generic;

namespace Client.Model
{
    class MapConverter
    {
        private const float WIDTH = 30;

        public List<DrawableField> ConvertToDrawable(List<Field> map)
        {
            List<DrawableField> drawableMap = new List<DrawableField>();

            float height = (float)(WIDTH * Math.Sqrt(4f / 3f));

            foreach (var item in map)
            {
                float x = (item.X * WIDTH) + item.Y * (WIDTH / 2);
                float y = item.Y * height - (item.Y * (height / 4f));
                drawableMap.Add(new DrawableField(x, y, WIDTH, height, item.Colour));
            }

            return drawableMap;
        }
    }
}
