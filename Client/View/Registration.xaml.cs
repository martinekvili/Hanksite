using Client.View.Interfaces;
using Client.ViewModel;
using System.Windows.Controls;
using System;

namespace Client.View
{
    public partial class Registration : UserControl, IConfirmedPasswordProvider
    {
        private RegistrationViewModel viewModel;

        public Registration()
        {
            InitializeComponent();
            viewModel = (RegistrationViewModel)base.DataContext;
            viewModel.View = this;
        }

        public string GetPassword()
        {
            return passwordBox.Password;
        }

        public string GetConfirmedPassword()
        {
            return confirmedPasswordBox.Password;
        }
    }
}
