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
using log4net;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public partial class HanksiteSession : IHanksiteService
    {
        private readonly ILog logger;
        private readonly IHanksiteServiceCallback callback;

        public HanksiteSession()
        {
            logger = LogManager.GetLogger(nameof(HanksiteSession));
            callback = OperationContext.Current.GetCallbackChannel<IHanksiteServiceCallback>();

            OperationContext.Current.InstanceContext.Closed += InstanceContext_Closed;
        }

        public void Ping() { }

        private void log(string message)
        {
            logger.Info($"Client '{user?.UserName ?? "unknown"}' {message}.");
        }

        private void logError(string message, Exception ex)
        {
            logger.Error($"Error during {message} to client '{user?.UserName ?? "unknown"}'.", ex);
        }

        private void InstanceContext_Closed(object sender, EventArgs e)
        {
            log("disconnected");

            if (LobbyMember != null)
                DisconnectFromLobby();

            if (RealPlayer != null)
                DisconnectFromGame();
        }
    }
}
