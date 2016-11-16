using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Server.Game.Player;
using Server.Game;
using GameSnapshot = Common.Game.GameSnapshot;
using GameSnapshotForNextPlayer = Common.Game.GameSnapshotForNextPlayer;

namespace Server
{
    public partial class HanksiteSession
    {
        public RealPlayer RealPlayer { get; set; }

        public void ChooseColour(int colour)
        {
            RealPlayer.ChooseColour(colour);
        }

        public void DisconnectFromGame()
        {
            RealPlayer.DisconnectFromGame();

            RealPlayer = null;
        }

        public GameSnapshotForDisconnected[] GetRunningGames()
        {
            return GameManagerRepository.Instance.GetGamesForPlayer(user.ID);
        }

        public GameSnapshot ReconnectToGame(int gameId)
        {
            GameManager game = GameManagerRepository.Instance.GetGameByID(gameId);

            if (game == null)
                return null;

            return game.ReconnectToGame(this);
        }

        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            callback.SendGameSnapshot(snapshot);
        }

        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            callback.DoNextStep(snapshot);
        }

        public void SendGamePlayersSnapshot(GamePlayersSnapshot snapshot)
        {
            callback.SendGamePlayerSnapshot(snapshot);
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            callback.SendGameOver(snapshot);

            RealPlayer = null;
        }
    }
}
