using Client.Model;
using Client.ViewModel;
using System.Windows.Controls;

namespace Client.View
{
    public partial class CreateLobby : UserControl
    {
        private CreateLobbyViewModel viewModel;

        public CreateLobby()
        {
            Initialize();
        }

        public CreateLobby(Lobby selectedLobby)
        {
            Initialize();
            viewModel.SetLobby(selectedLobby);
        }

        private void Initialize()
        {
            InitializeComponent();
            viewModel = (CreateLobbyViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
