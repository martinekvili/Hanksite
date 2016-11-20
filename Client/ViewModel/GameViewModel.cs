﻿using Client.Helper;
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
using System.Media;
using System.Windows.Resources;
using Client.Properties;
using System.Windows.Navigation;
using Client.View;
using Client.ServerConnection;
using System.Windows.Data;
using System.Globalization;

namespace Client.ViewModel
{
    class GameViewModel : INotifyPropertyChanged, IGameActions
    {
        private MapProvider mapProvider;
        private Dictionary<int, Color> colours;
        private MapConverter mapConverter;
        private MapAttributes mapAttributes;

        public float CanvasWidth { get; set; }
        public float CanvasHeight { get; set; }

        private List<DrawableField> map;
        public ObservableCollection<DrawableField> Map => new ObservableCollection<DrawableField>(map);

        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        #region counter
        public bool IsCounterRunning { get; set; }
        public int RemainingSeconds => remainingSecondsByRound[actualRound];
        public Brush CounterColour { get; set; }
        private SolidColorBrush defaultColour;
        private SolidColorBrush lastSecondsColour;
        private SoundPlayer soundPlayer;
        private int actualRound;
        private Dictionary<int, int> remainingSecondsByRound;
        #endregion

        public List<GamePlayer> Players { get; set; }
        public List<DrawableField> AvailableCells { get; set; }

        private IGameServer gameServer;

        public GameViewModel()
        {
            mapProvider = new MapProvider();
            colours = new ColourProvider().Colours;
            mapConverter = new MapConverter();

            CanvasWidth = 800;
            CanvasHeight = 650;

            mapAttributes = mapConverter.GetMapAttributes(mapProvider.GetMap());
            map = mapConverter.ConvertToDrawable(mapProvider.GetMap(), CanvasWidth, CanvasHeight);

            #region counter
            IsCounterRunning = false;
            defaultColour = new SolidColorBrush(Color.FromScRgb(1, 0, 0, 0));
            lastSecondsColour = new SolidColorBrush(Color.FromScRgb(1, 1, 0, 0));
            soundPlayer = new SoundPlayer(Resources.lastseconds);
            actualRound = 0;
            remainingSecondsByRound = new Dictionary<int, int>();
            remainingSecondsByRound[0] = 0;
            #endregion

            gameServer = ClientProxyManager.Instance;
            ClientProxyManager.Instance.RegisterGame(this);
        }

        #region counter
        private void StartCounter()
        {
            remainingSecondsByRound[actualRound] = 15;
            NotifyPropertyChanged("RemainingSeconds");

            IsCounterRunning = true;
            NotifyPropertyChanged("IsCounterRunning");

            CounterColour = defaultColour;
            NotifyPropertyChanged("CounterColour");
            
            Task.Factory.StartNew(() =>
            {
                int round = actualRound;
                while (round == actualRound && 0 < remainingSecondsByRound[round])
                {
                    Thread.Sleep(1000);

                    Console.WriteLine(round + " - " + actualRound);

                    if (round != actualRound)
                    {
                        break;
                    }

                    remainingSecondsByRound[round]--;
                    NotifyPropertyChanged("RemainingSeconds");

                    if (remainingSecondsByRound[round] <= 3)
                    {
                        CounterColour = lastSecondsColour;
                        NotifyPropertyChanged("CounterColour");

                        soundPlayer.Play();
                    }
                }
                
                if (round == actualRound)
                {
                    StopCounter();
                }
                remainingSecondsByRound.Remove(round);
            });
        }

        private void StopCounter()
        {
            Console.WriteLine("stop");
            actualRound++;
            remainingSecondsByRound[actualRound] = 0;
            AvailableCells = new List<DrawableField>();
            NotifyPropertyChanged("AvailableCells");
            IsCounterRunning = false;
            NotifyPropertyChanged("IsCounterRunning");
        }
        #endregion

        public void ChooseColour(string tag)
        {
            StopCounter();
            DrawableField chosenField = map.FirstOrDefault(field => field.Tag == tag);
            int chosenColour = DecodeColur(chosenField.Colour);
            gameServer.ChooseColour(chosenColour);
        }

        private int DecodeColur(Brush brush)
        {
            return colours.FirstOrDefault(x => x.Value == ((SolidColorBrush)brush).Color).Key;
        }

        #region callbacks
        public void SendGameSnapshot(GameState state)
        {
            RefreshGame(state);
        }

        public void DoNextStep(GameState state, List<Coordinate> availableCells)
        {
            actualRound++;
            AvailableCells = mapConverter.ConvertToDrawable(availableCells, mapAttributes, CanvasWidth, CanvasHeight);
            NotifyPropertyChanged("AvailableCells");

            RefreshGame(state);
            StartCounter();
        }

        public void SendGamePlayerSnapshot(List<GamePlayer> players)
        {
            Players = players;
            NotifyPropertyChanged("Players");
        }

        public void SendGameOver(GameState state)
        {
            RefreshGame(state);
            ClientProxyManager.Instance.RemoveGame();
            MessageBox.Show("Game Over", "Hanksite", MessageBoxButton.OK);
            NavigationService.GetNavigationService(View).Navigate(new MainMenu());
        }
        #endregion

        public void RefreshGame(GameState state)
        {
            map = mapConverter.ConvertToDrawable(state.Map, CanvasWidth, CanvasHeight);
            NotifyPropertyChanged("Map");

            Players = state.Players;
            NotifyPropertyChanged("Players");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ColourConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int colourNumber = int.Parse(values[0].ToString());
            return new SolidColorBrush(new ColourProvider().Colours[colourNumber]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
