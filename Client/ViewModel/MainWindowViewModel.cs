using Client.Helper;
using Client.Model;
using Client.View.Dialogs;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }
        
        public ICommand ChangeServerCommand { get; set; }
        public ICommand ConfirmServerCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        private bool isFrameEnabled = true;
        public bool IsFrameEnabled
        {
            get { return (currentServer != "") && isFrameEnabled; }
            set { isFrameEnabled = value; NotifyPropertyChanged(nameof(IsFrameEnabled)); }
        }

        private string currentServer = "";
        public string CurrentServer
        {
            get { return currentServer; }
            set { currentServer = value; NotifyPropertyChanged("CurrentServer"); NotifyPropertyChanged("IsFrameEnabled"); }
        }

        private bool isCurrentServerEnabled = false;
        public bool IsCurrentServerEnabled
        {
            get { return isCurrentServerEnabled; }
            set
            {
                isCurrentServerEnabled = value;

                if (isCurrentServerEnabled)
                {
                    IsChangeServerButtonVisible = false;
                    IsConfirmServerButtonVisible = true;
                    IsFrameEnabled = false;
                }
                else
                {
                    IsChangeServerButtonVisible = true;
                    IsConfirmServerButtonVisible = false;
                    IsFrameEnabled = true;
                }

                NotifyPropertyChanged(nameof(IsCurrentServerEnabled));
            }
        }

        private bool isChangeServerButtonVisible = true;
        public bool IsChangeServerButtonVisible
        {
            get { return isChangeServerButtonVisible; }
            set { isChangeServerButtonVisible = value; NotifyPropertyChanged("IsChangeServerButtonVisible"); }
        }

        private bool isConfirmServerButtonVisible = false;
        public bool IsConfirmServerButtonVisible
        {
            get { return isConfirmServerButtonVisible; }
            set { isConfirmServerButtonVisible = value; NotifyPropertyChanged(nameof(IsConfirmServerButtonVisible)); }
        }

        private bool isQuitButtonVisible = true;
        public bool IsQuitButtonVisible
        {
            get { return isQuitButtonVisible; }
            set { isQuitButtonVisible = value; NotifyPropertyChanged("IsQuitButtonVisible"); }
        }

        public MainWindowViewModel()
        {
            ChangeServerCommand = new CommandHandler(ChangeServer);
            ConfirmServerCommand = new CommandHandler(ConfirmServer);
            QuitCommand = new CommandHandler(Quit);

            if (File.Exists("lastlogin.xml"))
            {
                LoginDataManager loginDataManager = new LoginDataManager();
                CurrentServer = loginDataManager.LoadLastLogin().Server;
            }
        }

        private void ChangeServer()
        {
            IsCurrentServerEnabled = true;
        }

        private void ConfirmServer()
        {
            IsCurrentServerEnabled = false;
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
