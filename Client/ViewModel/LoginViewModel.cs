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
using System.IO;

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

        internal IAccountProvider Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }

        private LoginDataManager loginDataManager;

        public LoginViewModel()
        {
            loginDataManager = new LoginDataManager();

            Accounts = ClientProxyManager.Instance;

            if (File.Exists("lastlogin.xml"))
            {
                LoginData lastLoginData = loginDataManager.LoadLastLogin();
                Username = lastLoginData.Username;
            }

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

                LoginData loginData = new LoginData(window.GetServer(), Username);
                loginDataManager.SaveLastLogin(loginData);
            }
            else
            {
                MessageBox.Show("Wrong username or password!", "Hanksite", MessageBoxButton.OK);
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