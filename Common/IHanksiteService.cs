using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common.Lobby;

namespace Common
{
    [ServiceContract(CallbackContract = typeof(IHanksiteServiceCallback))]
    public interface IHanksiteService : ILobbyService
    {
        [OperationContract(IsOneWay = false)]
        bool Connect(string userName);
    }

    public interface IHanksiteServiceCallback : ILobbyServiceCallback
    {
    }
}
