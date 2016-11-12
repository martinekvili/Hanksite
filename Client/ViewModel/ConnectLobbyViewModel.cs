using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class ConnectLobbyViewModel
    {
        public DependencyObject View { get; set; }

        public ICommand BackCommand { get; set; }

        private IAvailableLobbyProvider availableLobbyProvider;

        public List<Lobby> AvailableLobbies { get { return availableLobbyProvider.GetLobbies(); } }

        public ConnectLobbyViewModel()
        {
            availableLobbyProvider = new Lobbies();
            BackCommand = new CommandHandler(Back, true);
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }
    }
}
