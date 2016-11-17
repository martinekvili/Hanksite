using Client.ViewModel;
using System.Windows;
using System;
using Client.ViewModel.Dialogs;

namespace Client.View.Dialogs
{
    public partial class ChangeServerDialog : Window
    {
        private ChangeServerDialogViewModel viewModel;

        public ChangeServerDialog(Window owner)
        {
            InitializeComponent();
            Owner = owner;
            viewModel = (ChangeServerDialogViewModel)base.DataContext;
            viewModel.View = this;
        }

        public string GetServer()
        {
            return viewModel.Server;
        }
    }
}
