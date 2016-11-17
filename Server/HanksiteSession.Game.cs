using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Server.Game.Player;

namespace Server
{
    public partial class HanksiteSession
    {
        public RealPlayer RealPlayer { get; set; }

        public void ChooseColour(int colour)
        {
            log($"chose colour: {colour}");
            RealPlayer.ChooseColour(colour);
        }

        public void DisconnectFromGame()
        {
            log("disconnected from game");
            RealPlayer.DisconnectFromGame();

            RealPlayer = null;
        }

        public GameSnapshotForDisconnected[] GetRunningGames()
        {
            log("queries the running games he disconnected from");
            return Server.Game.GameManagerRepository.Instance.GetGamesForPlayer(user.ID);
        }

        public GameSnapshot ReconnectToGame(int gameId)
        {
            log("tries to reconnect to a game");
            var game = Server.Game.GameManagerRepository.Instance.GetGameByID(gameId);

            if (game == null)
                return null;

            return game.ReconnectToGame(this);
        }

        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            try
            {
                log("is sent a game snapshot");
                callback.SendGameSnapshot(snapshot);
            }
            catch (Exception ex)
            {
                logError("sending game snapshot", ex);
            }
            
        }

        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            try
            {
                log("is next in line to choose colour");
                callback.DoNextStep(snapshot);
            }
            catch (Exception ex)
            {
                logError("sending NextPlayerSnapshot", ex);
            }

        }

        public void SendGamePlayersSnapshot(GamePlayersSnapshot snapshot)
        {
            try
            {
                log("is sent a snapshot of the players");
                callback.SendGamePlayerSnapshot(snapshot);
            }
            catch (Exception ex)
            {
                logError("sending snapshot of the players", ex);
            }     
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            try
            {
                log("is sent game over message");
                callback.SendGameOver(snapshot);
            }
            catch (Exception ex)
            {
                logError("sending game over message", ex);
            }

            RealPlayer = null;
        }
    }
}
