using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client.View
{
    public partial class GameHistory : UserControl
    {
        private GameHistoryViewModel viewModel;

        public GameHistory()
        {
            InitializeComponent();
            viewModel = (GameHistoryViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
