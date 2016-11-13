using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Game
{
    [DataContract]
    public class PlayedGameInfo
    {
        [DataMember]
        public string Name;

        [DataMember]
        public DateTime StartTime;

        [DataMember]
        public TimeSpan Duration;

        [DataMember]
        public int Position;

        [DataMember]
        public PlayerCore[] Enemies;
    }
}
