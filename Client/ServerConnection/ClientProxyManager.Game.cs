using Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ViewModel.Interfaces;

namespace Client.ServerConnection
{
    public partial class ClientProxyManager
    {
        public void RegisterGame(IGameActions game)
        {
            callback.Game = game;

            proxy.ClientReady();
        }

        public void RemoveGame()
        {
            callback.Game = null;
        }

        public void ChooseColour(int colour)
        {
            proxy.ChooseColour(colour);
        }

        public void DisconnectFromGame()
        {
            proxy.DisconnectFromGame();
        }

        public Task<GameStateForDisconnected[]> GetRunningGames()
        {
            return Task.Factory.StartNew(() => proxy.GetRunningGames().Select(game => game.ToViewModel()).ToArray());
        }

        public Task<GameState> ReconnectToGame(int gameId)
        {
            return Task.Factory.StartNew(() => proxy.ReconnectToGame(gameId)?.ToViewModel());
        }
    }
}
