using Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    public interface IGameServer
    {
        void ChooseColour(int colour);

        void DisconnectFromGame();

        GameSnapshotForDisconnected[] GetRunningGames();

        GameSnapshot ReconnectToGame(int gameId);
    }
}
