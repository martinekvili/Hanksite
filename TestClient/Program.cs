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
using Common.Game;

namespace TestClient
{
    class CallbackImpl : IHanksiteServiceCallback
    {
        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            writeStandings(snapshot.Players, player => player.Points);

            if (snapshot.AvailableCells == null)
            {
                Console.WriteLine("No available cells to select...");
            }
            else
            {
                var avaliableColours = snapshot.Map
                 .Where(cell => snapshot.AvailableCells.Any(coord => coord.X == cell.Coord.X && coord.Y == cell.Coord.Y))
                 .Select(cell => cell.Colour)
                 .Distinct();

                Console.WriteLine($"Available colours: {string.Join(" ", avaliableColours)}");
            }    
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            Console.WriteLine("Game is over!");
            writeStandings(snapshot.Players, player => player.Position);
        }

        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            writeStandings(snapshot.Players, player => player.Points);
        }

        private void writeStandings(Player[] players, Func<Player, int> what)
        {
            Console.WriteLine(
                $"Standings: {string.Join("; ", players.Select(player => $"{player.User.UserName} - {what(player)}"))}");
        }

        public void SendLobbyClosed()
        {
            Console.WriteLine("Disconnected from lobby.");
        }

        public void SendLobbyMembersSnapshot(LobbyMembersSnapshot lobbySnapshot)
        {
            Console.WriteLine($"Members: {string.Join(" ", lobbySnapshot.LobbyMembers.Select(member => member.UserName))}");
        }

        public void SendNotEnoughPlayers()
        {
            Console.WriteLine("Game not started, not enough players.");
        }

        public void SendTimedOut()
        {
            Console.WriteLine("You didn't choose fast enough!");
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

                while (true)
                {
                    string[] message = Console.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    if (message[0] == "register")
                    {
                        if (proxy.RegisterUser(message[1], message[2]))
                            Console.WriteLine("Congrats, you have registered.");
                        else
                            Console.WriteLine("user already exists");
                    }
                    if (message[0] == "login")
                    {
                        if (proxy.ConnectUser(message[1], message[2]))
                            Console.WriteLine("Congrats, you have logged in.");
                        else
                            Console.WriteLine("Invalid username or password.");
                    }
                    if (message[0] == "getplayedgames")
                    {
                        var playedGames = proxy.GetPlayedGames();
                        foreach (var playedGameInfo in playedGames)
                        {
                            Console.WriteLine(
                                $"Game: {playedGameInfo.Name}, Position: {playedGameInfo.Position}, Duration: {playedGameInfo.Duration.Seconds} seconds");
                        }
                    }
                    if (message[0] == "create")
                    {
                        proxy.CreateLobby(new LobbySettings
                        {
                            NumberOfColours = 8,
                            Name = message[1],
                            NumberOfPlayers = int.Parse(message[2]),
                            BotNumbers = new LobbySettingsBotNumber[] { new LobbySettingsBotNumber { Difficulty = AIDifficulty.Hard, Number = int.Parse(message[3]) } }
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
                    else if (message[0] == "disclobby")
                    {
                        proxy.DisconnectFromLobby();
                    }
                    else if (message[0] == "startgame")
                    {
                        proxy.StartGame();
                    }
                    else if (message[0] == "choosecolour")
                    {
                        proxy.ChooseColour(int.Parse(message[1]));
                    }
                    else if (message[0] == "discgame")
                    {
                        proxy.DisconnectFromGame();
                    }
                    else if (message[0] == "reconnectgame")
                    {
                        proxy.ReconnectToGame(int.Parse(message[1]));
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
