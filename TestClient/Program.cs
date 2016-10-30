using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write the address of the server [127.0.0.1]:");
            string serverAddress = Console.ReadLine().Trim();
            if (serverAddress == string.Empty)
                serverAddress = "localhost";

            IService1 proxy = new ChannelFactory<IService1>(new NetTcpBinding(), new EndpointAddress(new Uri($"net.tcp://{serverAddress}:8733/Service1/"))).CreateChannel();

            var ret = proxy.GetDataUsingDataContract(new CompositeType());
            Console.WriteLine(ret.StringValue);

            Console.ReadKey();
        }
    }
}
