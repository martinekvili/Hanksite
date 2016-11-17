using Common.Game;

namespace Client.ViewModel.Interfaces
{
    interface IGameActions
    {
        void SendGameSnapshot(GameSnapshot snapshot);
        
        void DoNextStep(GameSnapshotForNextPlayer snapshot);
        
        void SendGamePlayerSnapshot(GamePlayersSnapshot snapshot);
        
        void SendGameOver(GameSnapshot snapshot);
    }
}
