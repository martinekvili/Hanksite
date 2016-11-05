using System.Collections.Generic;

namespace Client.Model
{
    class Accounts
    {
        private Dictionary<string, string> accounts;

        public Accounts()
        {
            accounts = new Dictionary<string, string>();
            accounts["kornyek"] = "admin";
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
