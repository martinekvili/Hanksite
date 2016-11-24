using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Client.Model;
using Client.Model.Interfaces;
using Common;
using Common.Lobby;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Threading;
using Common.Users;
using System.Windows;

namespace Client.ServerConnection
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public partial class ClientProxyManager : IAccountProvider, IAvailableLobbyProvider, ILobbyServer, IGameServer, IGameInfoProvider
    {
        private static ClientProxyManager instance = new ClientProxyManager();

        public static ClientProxyManager Instance => instance;

        private string serverUrl;
        private User user;
        private string password;

        public long UserID => user.ID;
        public string UserName => user.UserName;

        private ClientCallback callback;
        private DuplexChannelFactory<IHanksiteService> channelFactory;
        private IHanksiteService proxy;
        private Timer pingTimer;

        private ClientProxyManager() { }

        private void getProxyForServer(string serverUrl)
        {
            closeProxy();

            if (serverUrl.IndexOf(":") == -1)
                serverUrl += ":7458";

            callback = new ClientCallback();
            var instanceContext = new InstanceContext(callback);

            EndpointIdentity identity = EndpointIdentity.CreateDnsIdentity("Hanksite Server");
            channelFactory = new DuplexChannelFactory<IHanksiteService>(
                instanceContext,
                new NetTcpBinding("hanksiteBinding"),
                new EndpointAddress(new Uri($"net.tcp://{serverUrl}/HanksiteService/"), identity)
            );

            channelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode =
                X509CertificateValidationMode.None;

            proxy = channelFactory.CreateChannel();
        }

        private void closeProxy()
        {
            if (channelFactory == null)
                return;

            pingTimer?.Dispose();
            pingTimer = null;

            try
            {
                if (channelFactory.State != CommunicationState.Faulted)
                {
                    ((IClientChannel) proxy).Close();
                    channelFactory.Close();
                }
            }
            catch
            {
                ((IClientChannel) proxy).Abort();
                channelFactory.Abort();
            }
        }

        private bool connectAndStoreCredentials(string serverUrl, string userName, string password, Func<string, string, User> doConnect)
        {
            getProxyForServer(serverUrl);
            User connectedUser = doConnect(userName, password);

            if (connectedUser == null)
                return false;

            this.serverUrl = serverUrl;
            this.user = connectedUser;
            this.password = password;

            this.pingTimer = new Timer(pingServer, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return true;
        }

        private void pingServer(object state)
        {
            try
            {
                proxy.Ping();
            }
            catch (Exception)
            {
                closeProxy();

                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show(Application.Current.MainWindow, "Connection lost.", "Hanksite", MessageBoxButton.OK);
                    Application.Current.Shutdown();
                });
            }
        }

        public Task<bool> CreateAccount(string serverUrl, string username, string password)
        {
            return Task.Factory.StartNew(
                () => connectAndStoreCredentials(serverUrl, username, password, (u, p) => proxy.RegisterUser(u, p)));
        }

        public Task<bool> IsAccountValid(string serverUrl, string username, string password)
        {
            return Task.Factory.StartNew(() =>
                    connectAndStoreCredentials(serverUrl, username, password, (u, p) => proxy.ConnectUser(u, p)));
        }

        public Task<bool> ChangePassword(string password, string newPassword)
        {
            return Task.Factory.StartNew(() => proxy.ChangePassword(password, newPassword));
        }
    }
}
