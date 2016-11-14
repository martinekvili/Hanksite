using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils
{
    public static class EncryptionUtils
    {
        private static readonly int iterationNumber = 10000;

        public static void EncryptPassword(string password, out string passwordSalt, out string encryptedPassword)
        {
            byte[] saltBytes = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }

            using (var crypto = new Rfc2898DeriveBytes(password, saltBytes, iterationNumber))
            {
                byte[] passwordBytes = crypto.GetBytes(32);

                passwordSalt = Convert.ToBase64String(saltBytes);
                encryptedPassword = Convert.ToBase64String(passwordBytes);
            }
        }

        public static bool IsPasswordForUser(this DAL.User dbUser, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(dbUser.PasswordSalt);

            using (var crypto = new Rfc2898DeriveBytes(password, saltBytes, iterationNumber))
            {
                string encryptedPassword = Convert.ToBase64String(crypto.GetBytes(32));

                return dbUser.Password == encryptedPassword;
            }
        }
    }
}
