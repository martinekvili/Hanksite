using Client.Model;
using Client.Model.Dummy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    class ConnectLobbyViewModel
    {
        private IAvailableLobbyProvider availableLobbyProvider;

        public List<Lobby> AvailableLobbies { get { return availableLobbyProvider.GetLobbies(); } }

        public ConnectLobbyViewModel()
        {
            availableLobbyProvider = new Lobbies();
        }
    }
}
