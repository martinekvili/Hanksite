using Client.Model;
using Client.Model.Dummy;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;
using Client.View;
using Client.Helper;

namespace Client.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private IAccountProvider accounts;
        private string message;
        
        public ICommand SignInCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand QuitCommand { get; set; }
        
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

            SignInCommand = new CommandHandler(SignIn, true);
            CreateAccountCommand = new CommandHandler(CreateAccount, true);
            QuitCommand = new CommandHandler(Quit, true);
        }

        private void SignIn()
        {
            if (accounts.IsAccountValid(Username, Password))
            {
                NavigationService.GetNavigationService(View).Navigate(new MainMenu());
            }
            else
            {
                Message = "Wrong username or password!";
            }
        }

        private void CreateAccount()
        {
            NavigationService.GetNavigationService(View).Navigate(new Registration());
        }

        private void Quit()
        {
            Application.Current.Shutdown();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}