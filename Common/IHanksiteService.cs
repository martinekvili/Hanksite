using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common.Game;
using Common.Lobby;
using Common.Users;

namespace Common
{
    [ServiceContract(CallbackContract = typeof(IHanksiteServiceCallback))]
    public interface IHanksiteService : IUserService, ILobbyService, IGameService
    {
        [OperationContract(IsOneWay = false)]
        void Ping();
    }

    public interface IHanksiteServiceCallback : ILobbyServiceCallback, IGameServiceCallback
    { }
}
