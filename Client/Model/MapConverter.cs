using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Client.Model
{
    class MapConverter
    {
        private const float FIELD_WIDTH = 50;
        private Dictionary<int, Color> colours;

        public List<DrawableField> ConvertToDrawable(List<Field> map)
        {
            colours = new ColourProvider().Colours;

            List<DrawableField> drawableMap = new List<DrawableField>();

            float fieldHeight = (float)(FIELD_WIDTH * Math.Sqrt(4f / 3f));

            int mapSize = GetMapSize(map);

            float centerPositionX = ((mapSize * 0.75f) + 0.5f) * FIELD_WIDTH;
            float centerPositionY = ((mapSize / 2) + 0.5f) * (fieldHeight * 0.75f);
            
            foreach (var item in map)
            {
                float x = (item.X * FIELD_WIDTH) + item.Y * (FIELD_WIDTH / 2) - centerPositionX;
                float y = item.Y * fieldHeight - (item.Y * (fieldHeight / 4f)) - centerPositionY;
                drawableMap.Add(new DrawableField(x, y, FIELD_WIDTH, fieldHeight, new SolidColorBrush(colours[item.Colour])));
            }

            return drawableMap;
        }

        private int GetMapSize(List<Field> map)
        {
            int mapSize = 0;

            foreach (var item in map)
            {
                if (mapSize < item.X)
                {
                    mapSize = item.X;
                }
            }

            return mapSize;
        }
    }
}
