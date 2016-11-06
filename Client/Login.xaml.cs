using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private LoginViewModel viewModel;

        public Login()
        {
            InitializeComponent();
            viewModel = (LoginViewModel) base.DataContext;
        }

        private void OnClickRegistrationButton(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.Navigate(new Registration());
        }

        private void OnClickSignInButton(object sender, RoutedEventArgs e)
        {
            if (viewModel.CanLogin())
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(new MainMenu());
            }

        }
    }
}
