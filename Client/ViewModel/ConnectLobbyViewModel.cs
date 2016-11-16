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
            set { selectedLobby = value; NotifyPropertyChanged("SelectedLobby"); }
        }

        public ConnectLobbyViewModel()
        {
            availableLobbyProvider = new Lobbies();
            BackCommand = new CommandHandler(Back, true);
            RefreshCommand = new CommandHandler(Refresh, true);
            ConnectCommand = new CommandHandler(Connect, true);
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }

        private void Refresh()
        {
            AvailableLobbies = availableLobbyProvider.GetLobbies();
        }

        private void Connect()
        {
            NavigationService.GetNavigationService(View).Navigate(new CreateLobby(selectedLobby));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
