using Client.Helper;
using Client.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        public ICommand OpenChangeServerDialogCommand { get; set; }

        private string server;
        public string Server
        {
            get { return server; }
            set { server = value; NotifyPropertyChanged("Server"); }
        }

        private bool isChangeServerButtonVisible;
        public bool IsChangeServerButtonVisible
        {
            get { return isChangeServerButtonVisible; }
            set { isChangeServerButtonVisible = value; NotifyPropertyChanged("IsChangeServerButtonVisible"); }
        }

        public MainWindowViewModel()
        {
            OpenChangeServerDialogCommand = new CommandHandler(OpenChangeServerDialog, true);
            IsChangeServerButtonVisible = true;
            Server = "kamuszerver";
        }

        private void OpenChangeServerDialog()
        {
            ChangeServerDialog dialog = new ChangeServerDialog((Window)View);
            if (dialog.ShowDialog() == true)
            {
                Server = dialog.GetServer();
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
