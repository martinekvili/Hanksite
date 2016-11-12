using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Dummy
{
    class GameHistory : IGameInfoProvider
    {
        private List<GameInfo> games;

        public GameHistory()
        {
            games = new List<GameInfo>();

            GameInfo game = new GameInfo();
            game.StartTime = new DateTime(2016, 11, 6, 23, 34, 15);
            game.Length = new TimeSpan(0, 34, 23);
            game.Place = 2;
            game.Enemies = new List<Player>() { new Player() { Username = "Pistike" }, new Player() { Username = "ivan" } };

            GameInfo game2 = new GameInfo();
            game2.StartTime = new DateTime(2016, 11, 6, 23, 39, 33);
            game2.Length = new TimeSpan(0, 26, 22);
            game2.Place = 1;
            game2.Enemies = new List<Player>() { new Player() { Username = "Sándor" }, new Player() { Username = "j4m35" } };
        }

        public List<GameInfo> GetGameInfos()
        {
            return games;
        }
    }
}
