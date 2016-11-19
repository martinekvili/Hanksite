using Client.Model;
using Common.Game;
using System.Collections.Generic;

namespace Client.ViewModel.Interfaces
{
    public interface IGameActions
    {
        void SendGameSnapshot(GameSnapshot snapshot);
        
        void DoNextStep(GameSnapshotForNextPlayer snapshot);
        
        void SendGamePlayerSnapshot(GamePlayersSnapshot snapshot);
        
        void SendGameOver(GameSnapshot snapshot);
    }
}
