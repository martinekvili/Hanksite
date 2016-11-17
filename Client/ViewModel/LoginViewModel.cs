using Client.Model;
using Client.Model.Dummy;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;
using Client.View;
using Client.Helper;
using Client.Model.Interfaces;
using Client.ServerConnection;
using Client.ViewModel.Interfaces;

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

        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged("Message"); }
        }

        internal IAccountProvider Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }

        public LoginViewModel()
        {
            Accounts = ClientProxyManager.Instance;
            Username = "kornyek";
            Password = "admin";

            SignInCommand = new CommandHandler(SignIn, true);
            CreateAccountCommand = new CommandHandler(CreateAccount, true);
        }

        private async void SignIn()
        {
            IServerChanger window = (IServerChanger)Window.GetWindow(View);

            if (await Accounts.IsAccountValid(window.GetServer(), Username, Password))
            {
                NavigationService.GetNavigationService(View).Navigate(new MainMenu());
                window.HideChangeServerButton();
                window.HideQuitButton();
            }
            else
            {
                Message = "Wrong username or password!";
            }
        }

        private void CreateAccount()
        {
            NavigationService.GetNavigationService(View).Navigate(new Registration());
            IServerChanger window = (IServerChanger)Window.GetWindow(View);
            window.HideQuitButton();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}