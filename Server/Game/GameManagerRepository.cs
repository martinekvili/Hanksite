using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Common.Lobby;

namespace Server.Game
{
    public class GameManagerRepository
    {
        private static readonly GameManagerRepository instance = new GameManagerRepository();

        public static GameManagerRepository Instance => instance;

        private readonly object syncObject = new object();

        private readonly List<GameManager> gameManagers;

        private GameManagerRepository()
        {
            gameManagers = new List<GameManager>();
        }

        public void CreateGame(List<HanksiteSession> realPlayers, LobbySettings settings)
        {
            lock (syncObject)
            {
                gameManagers.Add(new GameManager(realPlayers, settings));
            }
        }

        public GameSnapshotForDisconnected[] GetGamesForPlayer(long playerId)
        {
            lock (syncObject)
            {
                return gameManagers.Where(game => game.IsPlayerInGame(playerId)).Select(game => game.SnapshotForDisconnected).ToArray();
            }
        }

        public GameManager GetGameByID(int gameId)
        {
            lock (syncObject)
            {
                return gameManagers.SingleOrDefault(game => game.ID == gameId);
            }
        }

        public void RemoveGame(GameManager gameManager)
        {
            lock (syncObject)
            {
                gameManagers.Remove(gameManager);
            }
        }
    }
}
