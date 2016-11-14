using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL
{
    public static class UserDAL
    {
        public static User GetUser(long userId)
        {
            using (var db = new HanksiteEntities())
            {
                return (from user in db.Users
                        where user.ID == userId
                        select user)
                        .Single();
            }
        }

        public static User GetUserByName(string userName)
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

        public static void ChangeUserPassword(long userId, string passwordSalt, string password)
        {
            using (var db = new HanksiteEntities())
            {
                User userToChange = (from user in db.Users
                                     where user.ID == userId
                                     select user)
                                     .Single();

                userToChange.PasswordSalt = passwordSalt;
                userToChange.Password = password;

                db.SaveChanges();
            }
        }
    }
}
