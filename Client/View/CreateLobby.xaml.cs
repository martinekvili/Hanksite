using Client.ViewModel;
using System.Windows.Controls;

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
    }
}
