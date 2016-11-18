using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Client.View;
using System.Windows.Data;
using System;
using System.Globalization;
using Client.ServerConnection;

namespace Client.ViewModel
{
    class ConnectLobbyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        public ICommand BackCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }

        private bool isPageEnabled;
        public bool IsPageEnabled
        {
            get { return isPageEnabled; }
            set { isPageEnabled = value; NotifyPropertyChanged("IsPageEnabled"); }
        }

        private IAvailableLobbyProvider availableLobbyProvider;
        private List<Lobby> availableLobbies;
        public List<Lobby> AvailableLobbies
        {
            get { return availableLobbies; }
            set { availableLobbies = value; NotifyPropertyChanged("AvailableLobbies"); }
        }
        private Lobby selectedLobby;
        public Lobby SelectedLobby
        {
            get { return selectedLobby; }
            set { selectedLobby = value; NotifyPropertyChanged("SelectedLobby"); NotifyPropertyChanged("IsConnectable"); }
        }
        public bool IsConnectable
        {
            get { return (selectedLobby == null) ? false : true; }
        }

        private ILobbyServer lobbyServer;

        public ConnectLobbyViewModel()
        {
            IsPageEnabled = true;

            availableLobbyProvider = ClientProxyManager.Instance;
            BackCommand = new CommandHandler(Back, true);
            RefreshCommand = new CommandHandler(Refresh, true);
            ConnectCommand = new CommandHandler(Connect, true);

            lobbyServer = ClientProxyManager.Instance;
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }

        private async void Refresh()
        {
            IsPageEnabled = false;
            AvailableLobbies = await availableLobbyProvider.GetLobbies();
            IsPageEnabled = true;
        }

        private async void Connect()
        {
            IsPageEnabled = false;
            Lobby lobby = await lobbyServer.ConnectToLobby(selectedLobby.Name);
            NavigationService.GetNavigationService(View).Navigate(new CreateLobby(lobby));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FreeSpaceCounter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int maxPlayers = int.Parse(values[0].ToString());
            int connectedPlayers = ((List<Player>)values[1]).Count;
            int bots = ((Dictionary<BotDifficulty, int>)values[2]).Count;
            return maxPlayers - connectedPlayers - bots;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
