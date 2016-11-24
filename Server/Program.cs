using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Server.Utils;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            using (ServiceHost host = new ServiceHost(typeof(HanksiteSession)))
            {
                var certificate = CertificateManager.GetCertificate();
                host.Credentials.ServiceCertificate.Certificate = certificate;

                host.Open();

                LogManager.GetLogger(nameof(Program)).Info($"The service started.");

                string hostAddress = host.BaseAddresses.FirstOrDefault().AbsoluteUri;
                string addressToShow = hostAddress.Substring("net.tcp://".Length);
                addressToShow = addressToShow.Remove(addressToShow.Length - "/HanksiteService".Length - 1);

                Console.WriteLine("The service is ready at {0}", addressToShow);

                while (true)
                {
                    Console.Write("> ");
                    string command = Console.ReadLine().Trim();

                    if (command == "help")
                    {
                        Console.WriteLine("The following commands are available right now:");
                        Console.WriteLine("\thelp\t\tprints out this help");
                        Console.WriteLine("\tuptime\t\ttells for how long has the server been up");
                        Console.WriteLine("\tquit\t\tstops the server");
                    }
                    else if (command == "uptime")
                    {
                        Console.WriteLine("The server has been up for {0:dd} days {0:hh} hours {0:mm} minutes and {0:ss} seconds.", DateTime.Now - startTime);
                    }
                    else if (command == "quit")
                    {
                        break;
                    }
                    else if (command == String.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Unrecognized command: '{command}'. Please enter 'help' for help.");
                    }
                } 

                host.Close();
            }

            Console.WriteLine("Server shutting down");
            LogManager.GetLogger(nameof(Program)).Info($"The service stopped.");
        }
    }
}
