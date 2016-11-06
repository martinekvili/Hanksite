using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        private RegistrationViewModel viewModel;

        public Registration()
        {
            InitializeComponent();
            viewModel = (RegistrationViewModel)base.DataContext;
        }

        private void OnClickRegistrationButton(object sender, RoutedEventArgs e)
        {
            if (viewModel.CreateAccount())
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.GoBack();
            }
        }

        private void OnClickBackButton(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.GoBack();
        }
    }
}
