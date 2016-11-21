using Client.Helper;
using Client.Model;
using Client.Model.Dummy;
using Client.Model.Interfaces;
using Client.ViewModel.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Client.ServerConnection;
using Client.View;
using Client.View.Interfaces;

namespace Client.ViewModel
{
    class RegistrationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private IAccountProvider accounts;

        public ICommand CreateAccountCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public string Server { get; set; }
        public string Username { get; set; }

        private bool isPageEnabled;
        public bool IsPageEnabled
        {
            get { return isPageEnabled; }
            set { isPageEnabled = value; NotifyPropertyChanged("IsPageEnabled"); }
        }

        public RegistrationViewModel()
        {
            IsPageEnabled = true;
            accounts = ClientProxyManager.Instance;

            CreateAccountCommand = new CommandHandler(CreateAccount);
            BackCommand = new CommandHandler(Back);
        }

        private async void CreateAccount()
        {
            IServerChanger window = (IServerChanger)Window.GetWindow(View);

            if (Username == null || Username.Length == 0)
            {
                MessageBox.Show("Username is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            IConfirmedPasswordProvider passwordProvider = (IConfirmedPasswordProvider)View;
            string password = passwordProvider.GetPassword();
            string confirmedPassword = passwordProvider.GetConfirmedPassword();

            if (password == null || password.Length == 0)
            {
                MessageBox.Show("Password is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            if (password != confirmedPassword)
            {
                MessageBox.Show("Password differs from confirmed password!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            IsPageEnabled = false;
            window.Disable();

            bool success;
            try
            {
                success = await accounts.CreateAccount(window.GetServer(), Username, password);
            }
            catch (System.Exception e)
            {
                MessageBox.Show("An error has occured while connecting to the server.\n" + e.Message, "Hanksite", MessageBoxButton.OK);
                IsPageEnabled = true;
                window.Enable();
                return;
            }

            if (!success)
            {
                IsPageEnabled = true;
                window.Enable();
                MessageBox.Show("The given username already exists!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            window.Enable();
            NavigationService.GetNavigationService(View).Navigate(new MainMenu());
            window.HideChangeServerButton();
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
            IServerChanger window = (IServerChanger)Window.GetWindow(View);
            window.UnhideQuitButton();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
