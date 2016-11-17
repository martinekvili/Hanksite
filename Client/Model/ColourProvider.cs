using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client.Model
{
    class ColourProvider
    {
        public Dictionary<int, Color> Colours { get; }

        public ColourProvider()
        {
            Colours = new Dictionary<int, Color>();
            Colours[0] = Colors.MidnightBlue;
            Colours[1] = Colors.Firebrick;
            Colours[2] = Colors.SeaGreen;
            Colours[3] = Colors.DarkGoldenrod;
            Colours[4] = Colors.MediumTurquoise;
            Colours[5] = Colors.Sienna;
            Colours[6] = Colors.OliveDrab;
            Colours[7] = Colors.DarkSlateGray;
            Colours[8] = Colors.LightSalmon;
            Colours[9] = Colors.LightSlateGray;
            Colours[10] = Colors.YellowGreen;
            Colours[11] = Colors.RoyalBlue;
            Colours[12] = Colors.LightSalmon;
            Colours[13] = Colors.SteelBlue;
            Colours[14] = Colors.Tan;
            Colours[15] = Colors.LightSeaGreen;
            Colours[16] = Colors.DarkSlateBlue;
        }
    }
}
