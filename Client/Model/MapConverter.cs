using Common.Game;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Client.Model
{
    class MapConverter
    {
        private const float DEFAULT_FIELD_WIDTH = 50;
        private Dictionary<int, Color> colours;

        public List<DrawableField> ConvertToDrawable(List<Field> map, float canvasWidth, float canvasHeight)
        {
            int mapSize = GetMapSize(map);

            float fieldWidth = DEFAULT_FIELD_WIDTH * 12 / mapSize;
            float fieldHeight = (float)(fieldWidth * Math.Sqrt(4f / 3f));

            colours = new ColourProvider().Colours;

            List<DrawableField> drawableMap = new List<DrawableField>();

            float centerPositionX = ((mapSize * 0.75f) + 0.5f) * fieldWidth;
            float centerPositionY = ((mapSize / 2) + 0.5f) * (fieldHeight * 0.75f);

            foreach (var item in map)
            {
                float x = (item.X * fieldWidth) + item.Y * (fieldWidth / 2) - centerPositionX + (canvasWidth / 2);
                float y = item.Y * fieldHeight - (item.Y * (fieldHeight / 4f)) - centerPositionY + (canvasHeight / 2);
                drawableMap.Add(new DrawableField(new Coordinate(item.X, item.Y), x, y, fieldWidth, fieldHeight, new SolidColorBrush(colours[item.Colour])));
            }

            return drawableMap;
        }

        public List<DrawableField> ConvertToDrawable(List<Coordinate> coordinates, MapAttributes attributes, float canvasWidth, float canvasHeight)
        {
            List<DrawableField> drawableMap = new List<DrawableField>();

            foreach (var item in coordinates)
            {
                float x = (item.X * attributes.FieldWidth) + item.Y * (attributes.FieldWidth / 2) - attributes.CenterPositionX + (canvasWidth / 2);
                float y = item.Y * attributes.FieldHeight - (item.Y * (attributes.FieldHeight / 4f)) - attributes.CenterPositionY + (canvasHeight / 2);
                drawableMap.Add(new DrawableField(new Coordinate(item.X, item.Y), x, y, attributes.FieldWidth, attributes.FieldHeight, new SolidColorBrush(Color.FromScRgb(0.5f, 1, 1, 1))));
            }

            return drawableMap;
        }

        public MapAttributes GetMapAttributes(List<Field> map)
        {
            MapAttributes attributes = new MapAttributes();

            int mapSize = GetMapSize(map);
            attributes.MapSize = GetMapSize(map);

            attributes.FieldWidth = DEFAULT_FIELD_WIDTH * 12 / mapSize;
            attributes.FieldHeight = (float)(attributes.FieldWidth * Math.Sqrt(4f / 3f));

            attributes.CenterPositionX = ((attributes.MapSize * 0.75f) + 0.5f) * attributes.FieldWidth;
            attributes.CenterPositionY = ((attributes.MapSize / 2) + 0.5f) * (attributes.FieldHeight * 0.75f);

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
