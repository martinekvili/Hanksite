using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Model;

namespace Client.ServerConnection
{
    public static class ConversionHelper
    {
        #region To ViewModel

        public static List<Player> ToViewModel(this Common.Lobby.LobbyMembersSnapshot lobbyMembers)
        {
            return lobbyMembers.LobbyMembers.Select(user => new Player { Username = user.UserName }).ToList();
        }

        public static BotDifficulty ToViewModel(this Common.Game.AIDifficulty difficulty)
        {
            switch (difficulty)
            {
                case Common.Game.AIDifficulty.Easy:
                    return BotDifficulty.EASY;
                case Common.Game.AIDifficulty.Medium:
                    return BotDifficulty.MEDIUM;
                case Common.Game.AIDifficulty.Hard:
                    return BotDifficulty.HARD;
                default:
                    throw new ArgumentException("Unkown AIDifficulty");
            }
        }

        public static Lobby ToViewModel(this Common.Lobby.LobbySettingsWithMembersSnapshot snapshot)
        {
            return new Lobby
            {
                Name = snapshot.Name,
                NumberOfPlayers = snapshot.NumberOfPlayers,
                NumberOfColours = snapshot.NumberOfColours,
                Bots = snapshot.BotNumbers.ToDictionary(botNumber => botNumber.Difficulty.ToViewModel(), botNumber => botNumber.Number),
                ConnectedPlayers = snapshot.LobbyMembers.Select(user => new Player {Username = user.UserName}).ToList()
            };
        }

        public static Coordinate ToViewModel(this Common.Game.Coord coord)
        {
            return new Coordinate(coord.X, coord.Y);
        }

        public static GameState ToViewModel(this Common.Game.GameSnapshot snapshot)
        {
            return new GameState
            {
                Map = snapshot.Map.Select(field => new Field(field)).ToList(),
                Players = snapshot.Players.Select(player => player.ToViewModel()).ToList()
            };
        }

        public static GamePlayer ToViewModel(this Common.Game.Player player)
        {
            GamePlayer viewModel = null;

            if (player is Common.Game.AIPlayer)
            {
                viewModel = new BotPlayer
                {
                    difficulty = (player as Common.Game.AIPlayer).Difficulty.ToViewModel()
                };
            }
            else
            {
                viewModel = new GamePlayer();
            }

            setFieldsInGamePlayer(viewModel, player);
            return viewModel;
        }

        private static void setFieldsInGamePlayer(GamePlayer viewModel, Common.Game.Player player)
        {
            viewModel.Username = player.User.UserName;
            viewModel.Colour = player.Colour;
            viewModel.Points = player.Points;
            viewModel.Position = player.Position;
            viewModel.Type = player.Type.ToViewModel();
            viewModel.ID = player.User.ID;
        }

        public static PlayerType ToViewModel(this Common.Game.PlayerType type)
        {
            switch (type)
            {
                case Common.Game.PlayerType.RealPlayer:
                    return PlayerType.REAL;
                case Common.Game.PlayerType.DisconnectedPlayer:
                    return PlayerType.DISCONNECTED;
                case Common.Game.PlayerType.AI:
                    return PlayerType.BOT;
                default:
                    throw new ArgumentException("Unknown PlayerType");
            }
        }

        public static GameStateForDisconnected ToViewModel(this Common.Game.GameSnapshotForDisconnected snapshot)
        {
            return new GameStateForDisconnected
            {
                ID = snapshot.ID,
                Name = snapshot.Name,
                Players = snapshot.Players.Select(user => new Player { Username = user.UserName }).ToList()
            };
        }

        public static GameInfo ToViewModel(this Common.Game.PlayedGameInfo game)
        {
            return new GameInfo
            {
                Name = game.Name,
                Place = game.Position,
                StartTime = game.StartTime,
                Length = game.Duration,
                Enemies = game.Enemies.Select(player => new Player { Username = player.User.UserName }).ToList()
            };
        }

        #endregion

        #region To DTO

        public static Common.Lobby.LobbySettings ToDto(this Lobby settings)
        {
            return new Common.Lobby.LobbySettings
            {
                Name = settings.Name,
                NumberOfPlayers = settings.NumberOfPlayers,
                NumberOfColours = settings.NumberOfColours,
                BotNumbers = new Common.Lobby.LobbySettingsBotNumber[]
                {
                    new Common.Lobby.LobbySettingsBotNumber { Difficulty = Common.Game.AIDifficulty.Easy, Number = settings.Bots[BotDifficulty.EASY] },
                    new Common.Lobby.LobbySettingsBotNumber { Difficulty = Common.Game.AIDifficulty.Medium, Number = settings.Bots[BotDifficulty.MEDIUM] },
                    new Common.Lobby.LobbySettingsBotNumber { Difficulty = Common.Game.AIDifficulty.Hard, Number = settings.Bots[BotDifficulty.HARD] }
                }
            };
        } 

        #endregion
    }
}
