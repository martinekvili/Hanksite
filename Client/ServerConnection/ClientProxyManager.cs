using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ServerConnection
{
    public class ClientProxyManager
    {
        private static ClientProxyManager instance = new ClientProxyManager();

        public static ClientProxyManager Instance => instance;


    }
}
