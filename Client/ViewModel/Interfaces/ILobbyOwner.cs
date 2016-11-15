using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel.Interfaces
{
    interface ILobbyOwner
    {
        void RefreshConnectedPlayers(List<Player> players);

        void NotEnoughPlayers();
    }
}
