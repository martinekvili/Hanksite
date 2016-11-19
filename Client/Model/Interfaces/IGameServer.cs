using Common.Game;

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
