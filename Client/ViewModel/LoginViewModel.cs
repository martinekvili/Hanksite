using Client.Model.Dummy;

namespace Client.ViewModel
{
    public class LoginViewModel
    {
        private Accounts accounts;

        public string Username { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            accounts = new Accounts();
            Username = "kornyek";
            Password = "admin";
        }

        public bool CanLogin()
        {
            if (Username == null || Password == null)
            {
                return false;
            }

            return accounts.IsAccountValid(Username, Password);
        }
    }
}