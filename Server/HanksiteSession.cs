using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Common.Lobby;
using Common.Users;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public partial class HanksiteSession : IHanksiteService
    {
        private readonly IHanksiteServiceCallback callback;

        private User user;
        public User User => user;

        public HanksiteSession()
        {
            callback = OperationContext.Current.GetCallbackChannel<IHanksiteServiceCallback>();

            OperationContext.Current.InstanceContext.Closed += InstanceContext_Closed;
        }

        public bool Connect(string userName)
        {
            Console.WriteLine($"Client '{userName}' connected");
            user = new User { ID = 0, UserName = userName };

            return true;
        }

        private void InstanceContext_Closed(object sender, EventArgs e)
        {
            Console.WriteLine($"Client '{user.UserName}' disconnected");

            if (LobbyMember != null)
                LobbyMember.DisconnectFromLobby();
        }
    }
}
