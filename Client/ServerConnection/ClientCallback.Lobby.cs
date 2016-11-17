using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Client.ViewModel.Interfaces;
using Common.Lobby;
using Client.Model;

namespace Client.ServerConnection
{
    public partial class ClientCallback
    {
        public ILobbyActions Lobby { get; set; }

        public void SendLobbyClosed()
        {
            if (Lobby != null)
            {
                Lobby = null;

                Application.Current.Dispatcher.InvokeAsync(() => Lobby.SendLobbyClosed());
            }    
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbySnapshot)
        {
            if (Lobby != null)
            {
                var players = lobbySnapshot.LobbyMembers.Select(user => new Player { Username = user.UserName }).ToList();
                Application.Current.Dispatcher.InvokeAsync(() => Lobby.SendLobbyMembersSnapshot(players));
            }
        }

        public void SendNotEnoughPlayers()
        {
            if (Lobby != null)
            {
                Application.Current.Dispatcher.InvokeAsync(() => Lobby.SendLobbyClosed());
            }
        }
    }
}
