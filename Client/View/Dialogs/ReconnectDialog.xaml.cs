using Client.Model;
using Client.ViewModel.Dialogs;
using System.Windows;

namespace Client.View.Dialogs
{
    public partial class ReconnectDialog : Window
    {
        private ReconnectDialogViewModel viewModel;

        public ReconnectDialog(Window owner, GameStateForDisconnected[] games)
        {
            InitializeComponent();
            Owner = owner;
            viewModel = (ReconnectDialogViewModel)base.DataContext;
            viewModel.View = this;
            viewModel.Games = games;
        }

        public GameState GetConnectedGameState()
        {
            return viewModel.ConnectedGameState;
        }
    }
}
