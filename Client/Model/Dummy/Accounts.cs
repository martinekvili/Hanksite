using Client.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace Client.Model.Dummy
{
    class Accounts : IAccountProvider
    {
        private Dictionary<string, string> accounts;

        public Accounts()
        {
            accounts = new Dictionary<string, string>();
            CreateAccount("kornyek", "admin");
            CreateAccount("meres", "LaborImage");
        }

        public bool CreateAccount(string username, string password)
        {
            if (accounts.ContainsKey(username))
            {
                return false;
            }

            accounts[username] = password;
            return true;
        }

        public bool IsAccountValid(string username, string password)
        {
            if (accounts.ContainsKey(username) && accounts[username] == password)
            {
                return true;
            }

            return false;
        }
    }
}
