using Client.View.Interfaces;
using Client.ViewModel;
using System.Windows.Controls;
using System;

namespace Client.View
{
    public partial class Login : UserControl, IPasswordProvider
    {
        private LoginViewModel viewModel;

        public Login()
        {
            InitializeComponent();
            viewModel = (LoginViewModel)base.DataContext;
            viewModel.View = this;
        }

        public string GetPassword()
        {
            return passwordBox.Password;
        }
    }
}
