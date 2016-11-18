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
using Client.View.Interfaces;

namespace Client.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private IAccountProvider accounts;

        public ICommand SignInCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }

        public string Server { get; set; }
        public string Username { get; set; }

        private bool isPageEnabled;
        public bool IsPageEnabled
        {
            get { return isPageEnabled; }
            set { isPageEnabled = value; NotifyPropertyChanged("IsPageEnabled"); }
        }

        internal IAccountProvider Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }

        private LoginDataManager loginDataManager;

        public LoginViewModel()
        {
            IsPageEnabled = true;
            loginDataManager = new LoginDataManager();

            Accounts = ClientProxyManager.Instance;

            if (File.Exists("lastlogin.xml"))
            {
                LoginData lastLoginData = loginDataManager.LoadLastLogin();
                Username = lastLoginData.Username;
            }

            SignInCommand = new CommandHandler(SignIn);
            CreateAccountCommand = new CommandHandler(CreateAccount);
        }

        private async void SignIn()
        {
            IServerChanger window = (IServerChanger)Window.GetWindow(View);

            if (Username == null || Username.Length == 0)
            {
                MessageBox.Show("Username is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            string password = ((IPasswordProvider)View).GetPassword();
            if (password == null || password.Length == 0)
            {
                MessageBox.Show("Password is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            IsPageEnabled = false;
            window.Disable();
            if (await Accounts.IsAccountValid(window.GetServer(), Username, password))
            {
                NavigationService.GetNavigationService(View).Navigate(new MainMenu());
                window.HideChangeServerButton();
                window.HideQuitButton();

                LoginData loginData = new LoginData(window.GetServer(), Username);
                loginDataManager.SaveLastLogin(loginData);
                window.Enable();
            }
            else
            {
                IsPageEnabled = true;
                window.Enable();
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