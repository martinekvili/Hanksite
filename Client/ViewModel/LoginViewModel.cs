using Client.Model;
using Client.Model.Dummy;
using System.ComponentModel;

namespace Client.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private IAccountProvider accounts;
        private string message;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged("Message"); }
        }

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

            if (!accounts.IsAccountValid(Username, Password))
            {
                Message = "Wrong username or password!";
                return false;
            }

            return true;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}