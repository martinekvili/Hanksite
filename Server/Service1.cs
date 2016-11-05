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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Service1 : IService1
    {
        private static int clientCounter = 0;

        private int clientNum;
        private IService1DuplexCallback callback;

        public Service1()
        {
            clientNum = Interlocked.Increment(ref clientCounter);
            callback = OperationContext.Current.GetCallbackChannel<IService1DuplexCallback>();

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
            Console.WriteLine($"Client No. #{clientNum} disconnected");
        }


    }
}
