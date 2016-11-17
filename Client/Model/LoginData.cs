namespace Client.Model
{
    public class LoginData
    {
        public string Server;
        public string Username;

        public LoginData()
        {

        }

        public LoginData(string server, string username)
        {
            Server = server;
            Username = username;
        }
    }
}
