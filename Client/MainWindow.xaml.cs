using Client.ViewModel;
using System.Windows;
using Client.View.Interfaces;

namespace Client
{
    public partial class MainWindow : Window, IServerChanger
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (MainWindowViewModel)base.DataContext;
            viewModel.View = this;
        }

        public string GetServer()
        {
            return viewModel.CurrentServer;
        }

        public void HideChangeServerButton()
        {
            viewModel.IsChangeServerButtonVisible = false;
        }

        public void HideQuitButton()
        {
            viewModel.IsQuitButtonVisible = false;
        }

        public void UnhideQuitButton()
        {
            viewModel.IsQuitButtonVisible = true;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}
