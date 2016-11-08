using Client.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.View
{
    public partial class CreateLobby : UserControl
    {
        private CreateLobbyViewModel viewModel;

        public CreateLobby()
        {
            InitializeComponent();
            viewModel = (CreateLobbyViewModel) base.DataContext;
            viewModel.View = this;
        }

        private void OnChangedNumberOfPlayersSelection(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.RefreshNumberOfColours();
            }
        }

        private void OnClickBot(object sender, MouseButtonEventArgs e)
        {
            viewModel.RemoveBot(((ListBoxItem)sender).Content.ToString());
        }

        private void OnChangedBotSelection(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(sender);
        }
    }
}
