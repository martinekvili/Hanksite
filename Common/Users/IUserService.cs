using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.Game;

namespace Common.Users
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract(IsOneWay = false)]
        User ConnectUser(string userName, string password);

        [OperationContract(IsOneWay = false)]
        User RegisterUser(string userName, string password);

        [OperationContract(IsOneWay = false)]
        bool ChangePassword(string oldPassword, string newPassword);

        [OperationContract(IsOneWay = false)]
        PlayedGameInfo[] GetPlayedGames();
    }
}
