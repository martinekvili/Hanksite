using Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ViewModel.Interfaces;
using System.Windows;

namespace Client.ServerConnection
{
    public partial class ClientCallback
    {
        private IGameActions game;

        public IGameActions Game
        {
            set
            {
                lock (syncObject)
                    game = value;
            }
        }

        public void DoNextStep(GameSnapshotForNextPlayer snapshot)
        {
            lock (syncObject)
            {
                if (game != null)
                {
                    var viewModel = snapshot.ToViewModel();
                    var availableCells = snapshot.AvailableCells?.Select(coord => coord.ToViewModel()).ToList();

                    Application.Current.Dispatcher.InvokeAsync(() => game.DoNextStep(viewModel, availableCells));
                }
            }
        }

        public void SendGameOver(GameSnapshot snapshot)
        {
            lock (syncObject)
            {
                if (game != null)
                {
                    var viewModel = snapshot.ToViewModel();

                    Application.Current.Dispatcher.InvokeAsync(() => game.SendGameOver(viewModel));
                }
            }
        }

        public void SendGamePlayerSnapshot(GamePlayersSnapshot snapshot)
        {
            lock (syncObject)
            {
                if (game != null)
                {
                    var players = snapshot.Players.Select(player => player.ToViewModel()).ToList();

                    Application.Current.Dispatcher.InvokeAsync(() => game.SendGamePlayerSnapshot(players));
                }
            }
        }

        public void SendGameSnapshot(GameSnapshot snapshot)
        {
            lock (syncObject)
            {
                if (game != null)
                {
                    var viewModel = snapshot.ToViewModel();

                    Application.Current.Dispatcher.InvokeAsync(() => game.SendGameSnapshot(viewModel));
                }
            }
        }
    }
}
