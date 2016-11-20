using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class GameHistoryViewModel : INotifyPropertyChanged
    {
        public DependencyObject View { get; set; }

        public ICommand GameHistorySelectionChangedCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public List<GameInfo> GameHistory { get; set; }
        public GameInfo SelectedGameHistory { get; set; }
        public List<Player> Enemies { get; set; }

        private IGameInfoProvider historyProvider;

        public event PropertyChangedEventHandler PropertyChanged;

        public GameHistoryViewModel()
        {
            GameHistorySelectionChangedCommand = new CommandHandler(RefreshEnemies);
            BackCommand = new CommandHandler(Back);

            historyProvider = new GameHistory();

            GameHistory = historyProvider.GetGameInfos();
            NotifyPropertyChanged("GameHistory");
        }

        private void RefreshEnemies()
        {
            Enemies = SelectedGameHistory.Enemies;
            NotifyPropertyChanged("Enemies");
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
}
