using Common.Game;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Client.Model
{
    class MapConverter
    {
        private const float FIELD_WIDTH = 50;
        private float FIELD_HEIGHT => (float)(FIELD_WIDTH * Math.Sqrt(4f / 3f));
        private Dictionary<int, Color> colours;

        public List<DrawableField> ConvertToDrawable(List<Field> map, float canvasWidth, float canvasHeight)
        {
            colours = new ColourProvider().Colours;

            List<DrawableField> drawableMap = new List<DrawableField>();

            int mapSize = GetMapSize(map);

            float centerPositionX = ((mapSize * 0.75f) + 0.5f) * FIELD_WIDTH;
            float centerPositionY = ((mapSize / 2) + 0.5f) * (FIELD_HEIGHT * 0.75f);
            
            foreach (var item in map)
            {
                float x = (item.X * FIELD_WIDTH) + item.Y * (FIELD_WIDTH / 2) - centerPositionX + (canvasWidth / 2);
                float y = item.Y * FIELD_HEIGHT - (item.Y * (FIELD_HEIGHT / 4f)) - centerPositionY + (canvasHeight / 2);
                drawableMap.Add(new DrawableField(new Coordinate(item.X, item.Y), x, y, FIELD_WIDTH, FIELD_HEIGHT, new SolidColorBrush(colours[item.Colour])));
            }

            return drawableMap;
        }

        public List<DrawableField> ConvertToDrawable(List<Coordinate> coordinates, MapAttributes attributes, float canvasWidth, float canvasHeight)
        {
            List<DrawableField> drawableMap = new List<DrawableField>();

            foreach (var item in coordinates)
            {
                float x = (item.X * FIELD_WIDTH) + item.Y * (FIELD_WIDTH / 2) - attributes.CenterPositionX + (canvasWidth / 2);
                float y = item.Y * FIELD_HEIGHT - (item.Y * (FIELD_HEIGHT / 4f)) - attributes.CenterPositionY + (canvasHeight / 2);
                drawableMap.Add(new DrawableField(new Coordinate(item.X, item.Y), x, y, FIELD_WIDTH, FIELD_HEIGHT, new SolidColorBrush(Color.FromScRgb(0.5f, 1, 1, 1))));
            }

            return drawableMap;
        }

        public MapAttributes GetMapAttributes(List<Field> map)
        {
            MapAttributes attributes = new MapAttributes();

            attributes.MapSize = GetMapSize(map);
            attributes.CenterPositionX = ((attributes.MapSize * 0.75f) + 0.5f) * FIELD_WIDTH;
            attributes.CenterPositionY = ((attributes.MapSize / 2) + 0.5f) * (FIELD_HEIGHT * 0.75f);

            return attributes;
        }

        public List<Field> ConvertToFields(Hexagon[] map)
        {
            List<Field> fields = new List<Field>();

            foreach (var hexagon in map)
            {
                fields.Add(new Field(hexagon));
            }

            return fields;
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
