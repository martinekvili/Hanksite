using Client.ViewModel;
using System.Windows.Controls;

namespace Client.View
{
    public partial class Login : UserControl
    {
        private LoginViewModel viewModel;

        public Login()
        {
            InitializeComponent();
            viewModel = (LoginViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
