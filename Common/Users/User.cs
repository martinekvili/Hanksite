using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Users
{
    [DataContract]
    public class User
    {
        [DataMember]
        public long ID;

        [DataMember]
        public string UserName;
    }
}
