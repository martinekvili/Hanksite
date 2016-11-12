using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace Client.ViewModel
{
    class GameViewModel
    {
        public ObservableCollection<DrawableField> Map { get; set; }
        public ICommand TestCommand { get; set; }

        private IMapProvider mapProvider;
        private Dictionary<int, Color> colours;

        public GameViewModel()
        {
            mapProvider = new MapProvider();
            colours = new ColourProvider().Colours;

            MapConverter converter = new MapConverter();
            Map = new ObservableCollection<DrawableField>(converter.ConvertToDrawable(mapProvider.GetMap()));

            TestCommand = new ParameterizedCommandHandler(Test, true);
        }

        private void Test(object parameter)
        {
            UIElement e = parameter as UIElement;
            Console.WriteLine(parameter);
        }

        public void DecodeColor(Brush brush)
        {
            Console.WriteLine(colours.FirstOrDefault(x => x.Value == ((SolidColorBrush)brush).Color));
        }
    }
}
