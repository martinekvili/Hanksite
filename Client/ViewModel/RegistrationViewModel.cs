using Client.Model;
using Client.Model.Dummy;
using System.ComponentModel;

namespace Client.ViewModel
{
    class RegistrationViewModel : INotifyPropertyChanged
    {
        private IAccountProvider accounts;
        private string message;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged("Message"); }
        }

        public RegistrationViewModel()
        {
            accounts = new Accounts();
            Username = "meres";
            Password = "LaborImage";
            ConfirmedPassword = "LaborImage";
        }

        public bool CreateAccount()
        {
            if (Password != ConfirmedPassword)
            {
                Message = "Password differs from confirmed password!";
                return false;
            }

            bool success = accounts.CreateAccount(Username, Password);
            if (!success)
            {
                Message = "The given username is already exists!";
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
