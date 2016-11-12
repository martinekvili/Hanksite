using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    interface IAccountProvider
    {
        bool CreateAccount(string username, string password);

        bool IsAccountValid(string username, string password);
    }
}
