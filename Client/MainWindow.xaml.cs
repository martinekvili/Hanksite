using Client.ViewModel;
using Client.ViewModel.Interfaces;
using System.Windows;
using System;

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

        public void HideQuitButton()
        {
            viewModel.IsQuitButtonVisible = false;
        }

        public void UnhideQuitButton()
        {
            viewModel.IsQuitButtonVisible = true;
        }
    }
}
