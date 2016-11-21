using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Client.ServerConnection;
using System.Windows.Data;
using System;
using System.Globalization;

namespace Client.ViewModel
{
    class GameHistoryViewModel : INotifyPropertyChanged
    {
        public DependencyObject View { get; set; }

        public ICommand GameHistorySelectionChangedCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public List<GameInfo> GameHistory { get; set; }
        public GameInfo SelectedGameHistory { get; set; }
        private List<GamePlayer> enemies;
        public List<GamePlayer> Enemies
        {
            get { return enemies; }
            set { enemies = value.OrderBy(enemy => enemy.Position).ToList(); NotifyPropertyChanged(nameof(Enemies)); }
        }

        private IGameInfoProvider historyProvider;

        public event PropertyChangedEventHandler PropertyChanged;

        public GameHistoryViewModel()
        {
            GameHistorySelectionChangedCommand = new CommandHandler(RefreshEnemies);
            BackCommand = new CommandHandler(Back);

            historyProvider = ClientProxyManager.Instance;
            getGameHistory();
        }

        private async void getGameHistory()
        {
            GameHistory = await historyProvider.GetGameInfos();
            NotifyPropertyChanged("GameHistory");
        }

        private void RefreshEnemies()
        {
            Enemies = SelectedGameHistory.Enemies;
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StartTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime time = (DateTime)value;
            return time.ToString("yyyy. MM. dd. HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = (TimeSpan)value;
            return time.ToString("mm':'ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
