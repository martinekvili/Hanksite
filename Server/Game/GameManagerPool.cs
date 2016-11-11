using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GameManagerPool
    {
        private static readonly GameManagerPool instance;

        public static GameManagerPool Instance => instance;

        private readonly object syncObject = new object();

        private readonly List<GameManager> gameManagers;

        private GameManagerPool()
        {
            gameManagers = new List<GameManager>();
        }

        public void CreateGame()
        {
            
        }

        public bool IsPlayerInGame(int playerId)
        {
            lock (syncObject)
            {
                return gameManagers.Any(game => game.IsPlayerInGame(playerId));
            }
        }
    }
}
