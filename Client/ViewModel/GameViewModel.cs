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
using Client.ViewModel.Interfaces;
using Common.Game;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

namespace Client.ViewModel
{
    class GameViewModel : INotifyPropertyChanged, IGameActions
    {
        private List<DrawableField> map;
        public ObservableCollection<DrawableField> Map => new ObservableCollection<DrawableField>(map);

        public ICommand TestCommand { get; set; }

        private MapProvider mapProvider;
        private Dictionary<int, Color> colours;

        private MapConverter mapConverter;

        public event PropertyChangedEventHandler PropertyChanged;

        public GameViewModel()
        {
            mapProvider = new MapProvider();
            colours = new ColourProvider().Colours;

            mapConverter = new MapConverter();
            map = mapConverter.ConvertToDrawable(mapProvider.GetMap());

            TestCommand = new ParameterizedCommandHandler(Test, IsTestEnabled);

            SimulateGame();
        }

        private void Test(object parameter)
        {
            UIElement e = parameter as UIElement;
            Console.WriteLine(parameter);
        }

        private bool IsTestEnabled(object parameter)
        {
            return true;
        }

        public void DecodeColor(Brush brush)
        {
            Console.WriteLine(colours.FirstOrDefault(x => x.Value == ((SolidColorBrush)brush).Color));
        }

        #region callbacks
        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            List<Field> fields = mapConverter.ConvertToFields(snapshot.Map);
            map = mapConverter.ConvertToDrawable(fields);
            NotifyPropertyChanged("Map");
        }

        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            throw new NotImplementedException();
        }

        public void SendGamePlayerSnapshot(GamePlayersSnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
        #endregion

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SimulateGame()
        {
            //GameSnapshot snapshot = new GameSnapshot();
            //snapshot.Map = mapProvider.GetHexagonMap();
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(1000);
            //    SendGameSnapshot(snapshot);
            //});
        }
    }
}
