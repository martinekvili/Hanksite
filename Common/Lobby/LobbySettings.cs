using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Common.Users;

namespace Common.Lobby
{
    [DataContract]
    public class LobbySettingsBotNumber
    {
        [DataMember]
        public AIDifficulty Difficulty;

        [DataMember]
        public int Number;
    }

    [DataContract]
    public class LobbySettings
    {
        [DataMember]
        public string Name;

        [DataMember]
        public int NumberOfPlayers;

        [DataMember]
        public int NumberOfColours;

        [DataMember]
        public LobbySettingsBotNumber[] BotNumbers;
    }

    [DataContract]
    public class LobbyMembersSnapshot
    {
        [DataMember]
        public User[] LobbyMembers;
    }

    [DataContract]
    public class LobbySettingsWithMembersSnapshot : LobbySettings
    {
        [DataMember]
        public User[] LobbyMembers;
    }
}
