using Common.Game;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    public interface IGameServer
    {
        void ChooseColour(int colour);

        void DisconnectFromGame();

        Task<GameStateForDisconnected[]> GetRunningGames();

        Task<GameState> ReconnectToGame(int gameId);
    }
}
