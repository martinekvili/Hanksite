using Client.Helper;
using Client.Model.Interfaces;
using Client.ServerConnection;
using Client.View.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        public ICommand BackCommand { get; set; }
        public ICommand ChangeCommand { get; set; }

        private bool isPageEnabled;
        public bool IsPageEnabled
        {
            get { return isPageEnabled; }
            set { isPageEnabled = value; NotifyPropertyChanged(nameof(IsPageEnabled)); }
        }

        private IAccountProvider accountProvider;

        public SettingsViewModel()
        {
            IsPageEnabled = true;

            BackCommand = new CommandHandler(Back);
            ChangeCommand = new CommandHandler(Change);

            accountProvider = ClientProxyManager.Instance;
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }

        private async void Change()
        {
            IPasswordChanger passwordProvider = (IPasswordChanger)View;

            string password = passwordProvider.GetPassword();
            string newPassword = passwordProvider.GetNewPassword();
            string confirmedPassword = passwordProvider.GetConfirmedPassword();

            if (password == null || password.Length == 0)
            {
                MessageBox.Show("Password is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            if (newPassword == null || newPassword.Length == 0)
            {
                MessageBox.Show("New password is empty!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            if (newPassword != confirmedPassword)
            {
                MessageBox.Show("New password differs from confirmed password!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            IsPageEnabled = false;

            bool success;
            try
            {
                success = await accountProvider.ChangePassword(password, newPassword);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Connection lost.", "Hanksite", MessageBoxButton.OK);
                Application.Current.Shutdown();
                return;
            }

            IsPageEnabled = true;
            if (!success)
            {
                MessageBox.Show("Wrong password!", "Hanksite", MessageBoxButton.OK);
                return;
            }

            NavigationService.GetNavigationService(View).GoBack();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
