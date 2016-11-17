using Client.Model.Interfaces;
using Common.Game;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Client.Model.Dummy
{
    class MapProvider : IMapProvider
    {
        private List<Field> map;

        public MapProvider()
        {
            map = new List<Field>();
            Random rnd = new Random();
            for (int i = 0; i <= 12; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    if (6 <= i + j && i + j <= 18)
                    {
                        map.Add(new Field(i, j, rnd.Next(7)));
                    }
                }
            }
        }

        public List<Field> GetMap()
        {
            return map;
        }

        public Hexagon[] GetHexagonMap()
        {
            List<Hexagon> hexagons = new List<Hexagon>();
            Random rnd = new Random();

            for (int i = 0; i <= 12; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    if (6 <= i + j && i + j <= 18)
                    {
                        Hexagon hexagon = new Hexagon();
                        hexagon.Coord = new Coord() { X = i, Y = j };
                        hexagon.Colour = rnd.Next(7);
                        hexagons.Add(hexagon);
                    }
                }
            }

            return hexagons.ToArray();
        }
    }
}
