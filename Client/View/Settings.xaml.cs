using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client.View
{
    public partial class Settings : UserControl
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
    }
}
