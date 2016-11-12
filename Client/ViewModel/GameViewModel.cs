using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client.ViewModel
{
    class GameViewModel
    {
        public ObservableCollection<DrawableField> Map { get; set; }
        public ICommand TestCommand { get; set; }

        private IMapProvider mapProvider;

        public GameViewModel()
        {
            mapProvider = new MapProvider();

            MapConverter converter = new MapConverter();
            Map = new ObservableCollection<DrawableField>(converter.ConvertToDrawable(mapProvider.GetMap()));
            
            //foreach (var item in Map)
            //{
            //    Console.WriteLine(item.X + "-" + item.Y + " " + item.Width + "-" + item.Height);
            //}

            TestCommand = new CommandHandler(Test, true);
        }

        private void Test()
        {
            Console.WriteLine("yáy");
        }
    }
}
