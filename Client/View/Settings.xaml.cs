using Client.View.Interfaces;
using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;

namespace Client.View
{
    public partial class Settings : UserControl, IPasswordChanger
    {
        private SettingsViewModel viewModel;

        public Settings()
        {
            InitializeComponent();
            viewModel = (SettingsViewModel)base.DataContext;
            viewModel.View = this;
        }

        private void OnClickBackButton(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.GoBack();
        }

        public string GetPassword()
        {
            return passwordBox.Password;
        }

        public string GetNewPassword()
        {
            return newPasswordBox.Password;
        }

        public string GetConfirmedPassword()
        {
            return confirmedPasswordBox.Password;
        }
    }
}
