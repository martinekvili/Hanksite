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

namespace Client.ServerConnection
{
    [ServiceBehavior(UseSynchronizationContext = false)]
    public partial class ClientProxyManager : IAccountProvider, IAvailableLobbyProvider, ILobbyServer, IGameServer, IGameInfoProvider
    {
        private static ClientProxyManager instance = new ClientProxyManager();

        public static ClientProxyManager Instance => instance;

        private string serverUrl;
        private string userName;
        public string UserName => userName;
        private string password;

        private ClientCallback callback;
        private DuplexChannelFactory<IHanksiteService> channelFactory;
        private IHanksiteService proxy;

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

            var certificate = CertificateManager.GetCertificate();
            channelFactory.Credentials.ClientCertificate.Certificate = certificate;
            channelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode =
                X509CertificateValidationMode.None;

            proxy = channelFactory.CreateChannel();
        }

        private void closeProxy()
        {
            if (channelFactory == null)
                return;

            if (channelFactory.State != CommunicationState.Faulted)
            {
                channelFactory.Close();
            }
            else
            {
                channelFactory.Abort();
            }
        }

        private bool connectAndStoreCredentials(string serverUrl, string userName, string password, Func<string, string, bool> doConnect)
        {
            getProxyForServer(serverUrl);

            bool result = doConnect(userName, password);
            if (result)
            {
                this.serverUrl = serverUrl;
                this.userName = userName;
                this.password = password;
            }

            return result;
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

        public Task<bool> TryReconnect()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    getProxyForServer(serverUrl);
                    proxy.ConnectUser(userName, password);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<bool> ChangePassword(string password, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
