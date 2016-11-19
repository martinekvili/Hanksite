using Client.Model;
using Common.Game;
using System.Collections.Generic;

namespace Client.ViewModel.Interfaces
{
    public interface IGameActions
    {
        void SendGameSnapshot(GameState state);
        
        void DoNextStep(GameState state, List<Coordinate> availableCells);
        
        void SendGamePlayerSnapshot(List<GamePlayer> players);
        
        void SendGameOver(GameState state);
    }
}
