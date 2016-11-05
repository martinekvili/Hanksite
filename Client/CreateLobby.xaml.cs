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
    /// Interaction logic for CreateLobby.xaml
    /// </summary>
    public partial class CreateLobby : UserControl
    {
        private CreateLobbyViewModel viewModel;

        public CreateLobby()
        {
            InitializeComponent();
            viewModel = (CreateLobbyViewModel) base.DataContext;
        }

        private void OnClickBackButton(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.GoBack();
        }

        private void OnChangedNumberOfPlayersSelection(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnClickAddBotButton(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
