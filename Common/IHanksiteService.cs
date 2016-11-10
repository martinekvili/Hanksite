using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Common
{
    [ServiceContract(CallbackContract = typeof(IHanksiteServiceCallback))]
    public interface IHanksiteService
    {
        [OperationContract(IsOneWay = true)]
        void Connect();

        [OperationContract(IsOneWay = true)]
        void SendMessage(string message);

        // TODO: Add your service operations here
    }

    public interface IHanksiteServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Send(string message);
    }
}
