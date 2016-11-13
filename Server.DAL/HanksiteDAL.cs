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
