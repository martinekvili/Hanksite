using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ViewModel.Interfaces;
using Client.Model;

namespace Client.ServerConnection
{
    public partial class ClientProxyManager
    {
        public void RegisterLobby(ILobbyActions lobby)
        {
            callback.Lobby = lobby;
        }

        public void RemoveLobby()
        {
            callback.Lobby = null;
        }

        public Task<List<Lobby>> GetLobbies()
        {
            return Task.Factory.StartNew(() => proxy.ListLobbies().Select(lobby => lobby.ToViewModel()).ToList());
        }
    }
}
