using Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Game
{
    [DataContract]
    public class GameSnapshot
    {
        [DataMember]
        public string Name;

        [DataMember]
        public Player[] Players;

        [DataMember]
        public Hexagon[] Map;
    }

    [DataContract]
    public class GameSnapshotForNextPlayer : GameSnapshot
    {
        [DataMember]
        public Coord[] AvailableCells;
    }

    [DataContract]
    public class GameSnapshotForDisconnected
    {
        [DataMember]
        public int ID;

        [DataMember]
        public string Name;

        [DataMember]
        public User[] Players;
    }
}
