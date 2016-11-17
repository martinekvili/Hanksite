using Client.Helper;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel.Dialogs
{
    class ChangeServerDialogViewModel
    {
        public DependencyObject View { get; set; }

        public ICommand ChangeCommand { get; set; }

        public string Server { get; set; }

        public ChangeServerDialogViewModel()
        {
            ChangeCommand = new CommandHandler(Change, true);
        }

        private void Change()
        {
            ((Window)View).DialogResult = true;
        }
    }
}
