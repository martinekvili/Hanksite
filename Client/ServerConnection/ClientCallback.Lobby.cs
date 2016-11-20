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
        private ILobbyActions lobby;

        public ILobbyActions Lobby
        {
            set
            {
                lock (syncObject)
                    lobby = value;
            }
        }

        public void SendLobbyClosed()
        {
            lock (syncObject)
            {
                if (lobby != null)
                    Application.Current.Dispatcher.InvokeAsync(() => lobby.SendLobbyClosed());
            }
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbySnapshot)
        {
            lock (syncObject)
            {
                if (lobby != null)
                {
                    var players = lobbySnapshot.ToViewModel();
                    Application.Current.Dispatcher.InvokeAsync(() => lobby.SendLobbyMembersSnapshot(players));
                }
            }
        }

        public void SendNotEnoughPlayers()
        {
            lock (syncObject)
            {
                if (lobby != null)
                    Application.Current.Dispatcher.InvokeAsync(() => lobby.SendNotEnoughPlayers());
            }
        }

        public void SendGameStarted()
        {
            lock (syncObject)
            {
                if (lobby != null)
                    Application.Current.Dispatcher.InvokeAsync(() => lobby.SendGameStarted());
            }
        }
    }
}
