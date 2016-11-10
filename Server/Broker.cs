using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class Broker
    {
        private static readonly Broker instance = new Broker();
        public static Broker Instance => instance;

        private object syncObject = new object();
        private HashSet<IHanksiteServiceCallback> callbacks;

        public Broker()
        {
            callbacks = new HashSet<IHanksiteServiceCallback>();
        }

        public void AddCallback(IHanksiteServiceCallback callback)
        {
            lock(syncObject)
                callbacks.Add(callback);
        }

        public void RemoveCallback(IHanksiteServiceCallback callback)
        {
            lock (syncObject)
                callbacks.Remove(callback);
        }

        public void SendMessage(string message)
        {
            lock (syncObject)
            {
                foreach (var callback in callbacks)
                {
                    try
                    {
                        callback.Send(message);
                    }
                    catch (CommunicationException)
                    {
                        // The session has been closed but it has not had the time
                        // to inform the broker about it. Skipping this client.
                    }
                }
                    
            }
        }
    }
}
