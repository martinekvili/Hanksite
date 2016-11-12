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
            Colours[0] = Colors.CadetBlue;
            Colours[1] = Colors.Crimson;
            Colours[2] = Colors.DarkOliveGreen;
            Colours[3] = Colors.GreenYellow;
            Colours[4] = Colors.Indigo;
            Colours[5] = Colors.SlateGray;
            Colours[6] = Colors.SkyBlue;
        }
    }
}
