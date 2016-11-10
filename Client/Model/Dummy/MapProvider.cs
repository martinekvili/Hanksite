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
            for (int i = 0; i <= 12; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    if (6 <= i + j && i + j <= 18)
                    {
                        map.Add(new Field(i, j, new SolidColorBrush(Color.FromScRgb(1, i / 12f, j / 12f, 0))));
                    }
                }
            }
        }

        public List<Field> GetMap()
        {
            return map;
        }
    }
}
