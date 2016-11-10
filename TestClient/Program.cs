using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;
using Common.Lobby;

namespace TestClient
{
    class CallbackImpl : IHanksiteServiceCallback
    {
        public void SendLobbyClosed()
        {
            Console.WriteLine("Disconnected from lobby.");
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbySnapshot)
        {
            Console.WriteLine($"Members: {string.Join(" ", lobbySnapshot.LobbyMembers.Select(member => member.UserName))}");
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

                Console.WriteLine($"Connecting to {serverAddress}.");

                var instanceContext = new InstanceContext(new CallbackImpl());

                IHanksiteService proxy = new DuplexChannelFactory<IHanksiteService>(
                    instanceContext,
                    new NetTcpBinding("hanksiteBinding"),
                    new EndpointAddress(new Uri($"net.tcp://{serverAddress}:8733/HanksiteService/"))
                ).CreateChannel();

                Console.WriteLine("Enter username");
                proxy.Connect(Console.ReadLine().Trim());


                while (true)
                {
                    string[] message = Console.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (message[0] == "create")
                    {
                        proxy.CreateLobby(new LobbySettings
                        {
                            Name = message[1],
                            NumberOfPlayers = int.Parse(message[2]),
                            BotNumbers = new LobbySettingsBotNumber[] { }
                        });
                    }
                    else if (message[0] == "list")
                    {
                        var lobbies = proxy.ListLobbies();
                        foreach (var lobby in lobbies)
                        {
                            Console.WriteLine(lobby.Name);
                        }
                    }
                    else if (message[0] == "connect")
                    {
                        if (proxy.ConnectToLobby(message[1]) == null)
                            Console.WriteLine("Connect unsuccesful.");
                        else
                            Console.WriteLine("Connect succesful.");
                    }
                    else if (message[0] == "disconnect")
                    {
                        proxy.DisconnectFromLobby();
                    }
                    else if (message[0] == "exit")
                    {
                        break;
                    }
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
