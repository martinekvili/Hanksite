using Client.Model;
using Client.Model.Dummy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client.ViewModel
{
    class GameViewModel
    {
        public ObservableCollection<DrawableField> Map { get; set; }

        private IMapProvider mapProvider;

        public GameViewModel()
        {
            mapProvider = new MapProvider();

            MapConverter converter = new MapConverter();
            Map = new ObservableCollection<DrawableField>(converter.ConvertToDrawable(mapProvider.GetMap()));
            
            foreach (var item in Map)
            {
                Console.WriteLine(item.X + "-" + item.Y + " " + item.Width + "-" + item.Height);
            }
        }
    }
}
