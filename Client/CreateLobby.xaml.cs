using Client.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

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
            if (viewModel != null)
            {
                viewModel.refreshNumberOfColours();
            }
        }

        private void OnClickAddBotButton(object sender, RoutedEventArgs e)
        {
            viewModel.addSelectedBot();
        }

        private void OnClickBot(object sender, MouseButtonEventArgs e)
        {
            viewModel.removeBot(((ListBoxItem)sender).Content.ToString());
        }

        private void OnChangedBotSelection(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(sender);
        }

        private void OnClickStartButton(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.Navigate(new GameView());
        }

        private void OnClickReadyButton(object sender, RoutedEventArgs e)
        {
            viewModel.ready();
        }
    }
}
