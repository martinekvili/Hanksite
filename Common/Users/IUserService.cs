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
        bool ConnectUser(string userName, string password);

        [OperationContract(IsOneWay = false)]
        bool RegisterUser(string userName, string password);

        [OperationContract(IsOneWay = false)]
        PlayedGameInfo[] GetPlayedGames();
    }
}
