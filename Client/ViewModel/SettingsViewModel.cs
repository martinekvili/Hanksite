using Client.Helper;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel
{
    class SettingsViewModel
    {
        public DependencyObject View { get; set; }

        public ICommand BackCommand { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedPassword { get; set; }

        public SettingsViewModel()
        {
            BackCommand = new CommandHandler(Back);
        }

        private void Back()
        {
            NavigationService.GetNavigationService(View).GoBack();
        }
    }
}
