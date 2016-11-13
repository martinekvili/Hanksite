using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;

namespace Server.DAL
{
    public static class HanksiteDAL
    {
        public static User GetUser(string userName)
        {
            using (var db = new HanksiteEntities())
            {
                return (from user in db.Users
                        where user.UserName == userName
                        select user)
                        .SingleOrDefault();
            }
        }

        public static long RegisterUser(string userName, string passwordSalt, string password)
        {
            using (var db = new HanksiteEntities())
            {
                User user = new User
                {
                    UserName = userName,
                    PasswordSalt = passwordSalt,
                    Password = password
                };

                db.Users.Add(user);
                db.SaveChanges();

                return user.ID;
            }
        }

        public static List<PlayedGameInfo> GetPlayedGamesForUser(long userId)
        {
            using (var db = new HanksiteEntities())
            {
                var qUserGames = from userGame in db.GameUsers.Include("Game").Include("Game.GameUsers").Include("Game.GameUsers.User")
                                 where userGame.UserID == userId
                                 select userGame;

                List<PlayedGameInfo> playedGames = new List<PlayedGameInfo>();
                foreach (var userGame in qUserGames)
                {
                    playedGames.Add(new PlayedGameInfo
                    {
                        Position = (int)userGame.Position,
                        Name = userGame.Game.Name,
                        StartTime = userGame.Game.StartTime,
                        Duration = userGame.Game.EndTime - userGame.Game.StartTime,
                        Enemies = (from user in userGame.Game.GameUsers
                                   where user.UserID != userId
                                   select new PlayerCore
                                   {
                                       User = new Common.Users.User { ID = user.UserID, UserName = user.User.UserName },
                                       Position = (int)user.Position
                                   }).ToArray()
                    });
                }

                return playedGames;
            }
        }

        public static void StoreGame(GameSnapshot snapshot, DateTime startTime)
        {
            using (var db = new HanksiteEntities())
            {
                Game game = new Game
                {
                    Name = snapshot.Name,
                    StartTime = startTime,
                    EndTime = DateTime.Now
                };

                foreach (var player in snapshot.Players)
                {
                    GameUser gameUser = new GameUser
                    {
                        Game = game,
                        UserID = player.User.ID,
                        Position = player.Position
                    };
                    db.GameUsers.Add(gameUser);
                }

                db.SaveChanges();
            }
        }
    }
}
