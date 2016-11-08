using Client.ViewModel;
using System.Windows.Controls;

namespace Client.View
{
    public partial class Registration : UserControl
    {
        private RegistrationViewModel viewModel;

        public Registration()
        {
            InitializeComponent();
            viewModel = (RegistrationViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
