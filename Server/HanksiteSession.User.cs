using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Server.DAL;
using User = Common.Users.User;

namespace Server
{
    public partial class HanksiteSession
    {
        private static readonly int iterationNumber = 10000;

        private User user;
        public User User => user;

        public bool ConnectUser(string userName, string password)
        {
            var dbUser = HanksiteDAL.GetUser(userName);
            
            if (dbUser == null)
                return false;

            byte[] saltBytes = Convert.FromBase64String(dbUser.PasswordSalt);
            Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(password, saltBytes, iterationNumber);
            string passwordString = Convert.ToBase64String(crypto.GetBytes(32));

            if (passwordString != dbUser.Password)
                return false;

            user = new User { ID = dbUser.ID, UserName = userName };
            return true;
        }

        public bool RegisterUser(string userName, string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }

            Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(password, saltBytes, iterationNumber);
            byte[] passwordBytes = crypto.GetBytes(32);

            long id;
            try
            {
                id = HanksiteDAL.RegisterUser(userName, Convert.ToBase64String(saltBytes),
                    Convert.ToBase64String(passwordBytes));
            }
            catch (Exception)
            {
                return false;
            }

            user = new User { ID = id, UserName = userName };
            return true;
        }
    }
}
