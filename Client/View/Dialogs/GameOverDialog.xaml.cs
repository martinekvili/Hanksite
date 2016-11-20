using Client.Model;
using Client.ViewModel.Dialogs;
using System.Collections.Generic;
using System.Windows;

namespace Client.View.Dialogs
{
    public partial class GameOverDialog : Window
    {
        private GameOverDialogViewModel viewModel;

        public GameOverDialog(Window owner, List<GamePlayer> players)
        {
            InitializeComponent();
            Owner = owner;
            viewModel = (GameOverDialogViewModel)base.DataContext;
            viewModel.View = this;
            viewModel.Players = players;
        }
    }
}
