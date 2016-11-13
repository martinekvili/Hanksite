using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Common.Lobby;

namespace Server.Game
{
    public class GameManagerPool
    {
        private static readonly GameManagerPool instance = new GameManagerPool();

        public static GameManagerPool Instance => instance;

        private readonly object syncObject = new object();

        private readonly List<GameManager> gameManagers;

        private GameManagerPool()
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

        public GameSnapshot[] GetGamesForPlayer(long playerId)
        {
            lock (syncObject)
            {
                return gameManagers.Where(game => game.IsPlayerInGame(playerId)).Select(game => game.GameSnapshot).ToArray();
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
