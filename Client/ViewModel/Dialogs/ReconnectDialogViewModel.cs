using Client.Helper;
using Client.Model;
using Client.Model.Interfaces;
using Client.ServerConnection;
using Client.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Client.ViewModel.Dialogs
{
    public class ReconnectDialogViewModel : INotifyPropertyChanged
    {
        public DependencyObject View { get; set; }

        public ICommand ReconnectCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private GameStateForDisconnected[] games;
        public GameState ConnectedGameState { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public GameStateForDisconnected[] Games
        {
            get { return games; }
            set { games = value; NotifyPropertyChanged("Games"); }
        }
        public GameStateForDisconnected SelectedGame { get; set; }

        private IGameServer gameServer;

        public ReconnectDialogViewModel()
        {
            ReconnectCommand = new CommandHandler(Reconnect);
            CancelCommand = new CommandHandler(Cancel);

            gameServer = ClientProxyManager.Instance;
        }

        private async void Reconnect()
        {
            ConnectedGameState = await gameServer.ReconnectToGame(SelectedGame.ID);
            ((Window)View).DialogResult = true;
        }

        private void Cancel()
        {
            ((Window)View).DialogResult = false;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
