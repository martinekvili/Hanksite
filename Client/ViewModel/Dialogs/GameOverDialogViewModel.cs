using Client.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModel.Dialogs
{
    public class GameOverDialogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DependencyObject View { get; set; }

        private List<GamePlayer> players;
        public List<GamePlayer> Players
        {
            get { return players; }
            set
            {
                players = value.OrderBy(player => player.Position).ToList();
                NotifyPropertyChanged("Players");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
