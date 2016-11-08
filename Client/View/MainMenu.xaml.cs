using Client.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client.View
{
    public partial class MainMenu : UserControl
    {
        private MainMenuViewModel viewModel;

        public MainMenu()
        {
            InitializeComponent();
            viewModel = (MainMenuViewModel)base.DataContext;
            viewModel.View = this;
        }
    }
}
