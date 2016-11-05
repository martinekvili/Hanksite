using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;

namespace TestClient
{
    class CallbackImpl : IService1DuplexCallback
    {
        public void Send(string message)
        {
            Console.WriteLine($"Message received: {message}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Write the address of the server [127.0.0.1]:");
                string serverAddress = Console.ReadLine().Trim();
                if (serverAddress == string.Empty)
                    serverAddress = "localhost";

                Console.WriteLine($"Connecting to {serverAddress}. Write your messages, or write 'exit' to exit.");

                IService1 proxy = new DuplexChannelFactory<IService1>(
                    new InstanceContext(new CallbackImpl()),
                    new NetTcpBinding(SecurityMode.None),
                    new EndpointAddress(new Uri($"net.tcp://{serverAddress}:8733/Service1/"))
                ).CreateChannel();

                proxy.Connect();

                string message = Console.ReadLine().Trim();
                while (message != "exit")
                {
                    proxy.SendMessage(message);
                    message = Console.ReadLine().Trim();
                }
            }
            catch (CommunicationException)
            {
                Console.WriteLine("The server is unavailable, shutting down.");
                Console.ReadKey();
            }
        }
    }
}
