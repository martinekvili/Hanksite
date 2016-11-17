using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Lobby;

namespace Client.ServerConnection
{
    public partial class ClientCallback : IHanksiteServiceCallback
    {
        private readonly object syncObject = new object();
    }
}
