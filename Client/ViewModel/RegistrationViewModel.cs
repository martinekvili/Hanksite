using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class RegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private IAccountProvider accounts;
        private string message;

        public ICommand CreateAccountCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public string Server { get; set; }
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

            CreateAccountCommand = new CommandHandler(CreateAccount, true);
            BackCommand = new CommandHandler(Back, true);
        }

        private void CreateAccount()
        {
            if (Password != ConfirmedPassword)
            {
                Message = "Password differs from confirmed password!";
                return;
            }

            bool success = accounts.CreateAccount(Username, Password);
            if (!success)
            {
                Message = "The given username is already exists!";
                return;
            }

            NavigationService.GetNavigationService(View).GoBack();
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
