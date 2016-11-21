using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model.Interfaces
{
    public interface IAccountProvider
    {
        Task<bool> CreateAccount(string serverUrl, string username, string password);

        Task<bool> IsAccountValid(string serverUrl, string username, string password);

        Task<bool> ChangePassword(string password, string newPassword);
    }
}
