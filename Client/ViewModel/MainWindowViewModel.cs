using Client.Helper;
using Client.Model;
using Client.View.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Client.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        public ICommand OpenChangeServerDialogCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        public bool IsFrameEnabled => currentServer != "";

        private string currentServer = "";
        public string CurrentServer
        {
            get { return currentServer; }
            set { currentServer = value; NotifyPropertyChanged("CurrentServer"); NotifyPropertyChanged("IsFrameEnabled"); }
        }

        private bool isChangeServerButtonVisible;
        public bool IsChangeServerButtonVisible
        {
            get { return isChangeServerButtonVisible; }
            set { isChangeServerButtonVisible = value; NotifyPropertyChanged("IsChangeServerButtonVisible"); }
        }

        private bool isQuitButtonVisible;
        public bool IsQuitButtonVisible
        {
            get { return isQuitButtonVisible; }
            set { isQuitButtonVisible = value; NotifyPropertyChanged("IsQuitButtonVisible"); }
        }

        public MainWindowViewModel()
        {
            OpenChangeServerDialogCommand = new CommandHandler(OpenChangeServerDialog, true);
            QuitCommand = new CommandHandler(Quit, true);
            IsChangeServerButtonVisible = true;
            IsQuitButtonVisible = true;

            if(File.Exists("lastlogin.xml"))
            {
                LoginDataManager loginDataManager = new LoginDataManager();
                CurrentServer = loginDataManager.LoadLastLogin().Server;
            }
        }

        private void OpenChangeServerDialog()
        {
            ChangeServerDialog dialog = new ChangeServerDialog((Window)View);
            if (dialog.ShowDialog() == true)
            {
                CurrentServer = dialog.GetServer();
            }
        }

        private void Quit()
        {
            Application.Current.Shutdown();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
