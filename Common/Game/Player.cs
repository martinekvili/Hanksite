using Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Game
{
    public enum AIDifficulty
    {
        Hard,
        Medium,
        Easy
    }

    public enum PlayerType
    {
        RealPlayer,
        DisconnectedPlayer,
        AI
    }

    [DataContract]
    [KnownType(typeof(AIPlayer))]
    public class Player
    {
        [DataMember]
        public User User;

        [DataMember]
        public int Colour;

        [DataMember]
        public int Points;

        [DataMember]
        public int Position;

        [DataMember]
        public PlayerType Type;
    }

    [DataContract]
    public class AIPlayer : Player
    {
        [DataMember]
        public AIDifficulty Difficulty;
    }
}
