using Client.ViewModel;
using System.Windows.Controls;

namespace Client.View
{
    public partial class ConnectLobby : UserControl
    {
        private ConnectLobbyViewModel viewModel;
        public ConnectLobby()
        {
            InitializeComponent();
            viewModel = (ConnectLobbyViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
