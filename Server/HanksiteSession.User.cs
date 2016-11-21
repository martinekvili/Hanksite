using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Server.DAL;
using Server.Utils;

namespace Server
{
    public partial class HanksiteSession
    {
        private Common.Users.User user;
        public Common.Users.User User => user;

        public Common.Users.User ConnectUser(string userName, string password)
        {
            var dbUser = UserDAL.GetUserByName(userName);

            if (dbUser == null || !dbUser.IsPasswordForUser(password))
            {
                logger.Info($"Unsuccessful attempt to login with username '{userName}'.");
                return null;
            }

            user = new Common.Users.User { ID = dbUser.ID, UserName = userName };
            log("connected");
            return user;
        }

        public Common.Users.User RegisterUser(string userName, string password)
        {
            try
            {
                string passwordSalt, encryptedPassword;
                EncryptionUtils.EncryptPassword(password, out passwordSalt, out encryptedPassword);

                long id = UserDAL.RegisterUser(userName, passwordSalt, encryptedPassword);

                user = new Common.Users.User { ID = id, UserName = userName };
                log("registered");
                return user;
            }
            catch (Exception)
            {
                logger.Info($"Unsuccessful attept to register with username '{userName}'.");
                return null;
            }
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            log("tries to change password");

            var dbUser = UserDAL.GetUser(user.ID);

            if (!dbUser.IsPasswordForUser(oldPassword))
                return false;

            string passwordSalt, encryptedPassword;
            EncryptionUtils.EncryptPassword(newPassword, out passwordSalt, out encryptedPassword);

            UserDAL.ChangeUserPassword(user.ID, passwordSalt, encryptedPassword);
            return true;
        }

        public PlayedGameInfo[] GetPlayedGames()
        {
            log("queries the played games");
            return GameDAL.GetPlayedGamesForUser(user.ID).ToArray();
        }
    }
}
