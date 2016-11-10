using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class HanksiteSession : IHanksiteService
    {
        private static int clientCounter = 0;

        private int clientNum;
        private IHanksiteServiceCallback callback;

        public HanksiteSession()
        {
            clientNum = Interlocked.Increment(ref clientCounter);
            callback = OperationContext.Current.GetCallbackChannel<IHanksiteServiceCallback>();

            OperationContext.Current.InstanceContext.Closed += InstanceContext_Closed;
        }

        public void Connect()
        {
            Broker.Instance.AddCallback(callback);

            Console.WriteLine($"Client No. #{clientNum} connected");
            callback.Send($"Congrats, you are the client No. #{clientNum}");
        }

        public void SendMessage(string message)
        {
            Broker.Instance.SendMessage(message);
        }

        private void InstanceContext_Closed(object sender, EventArgs e)
        {
            Broker.Instance.RemoveCallback(callback);
            Console.WriteLine($"Client No. #{clientNum} disconnected");
        }


    }
}
