using Client.ViewModel;
using Client.ViewModel.Interfaces;
using System.Windows;

namespace Client
{
    public partial class MainWindow : Window, IHideableButtonContainer
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (MainWindowViewModel)base.DataContext;
            viewModel.View = this;
        }

        public void HideChangeServerButton()
        {
            viewModel.IsChangeServerButtonVisible = false;
        }
    }
}
