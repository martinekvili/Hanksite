using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Game
{
    [DataContract]
    public class Coord
    {
        [DataMember]
        public int X;

        [DataMember]
        public int Y;
    }

    public class Hexagon
    {
        [DataMember]
        public Coord Coord;

        [DataMember]
        public long OwnerID;

        [DataMember]
        public int Colour;
    }
}
