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
