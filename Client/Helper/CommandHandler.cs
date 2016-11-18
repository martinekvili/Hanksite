using System;
using System.Windows.Input;

namespace Client.Helper
{
    class CommandHandler : ICommand
    {
        private Action action;

        public event EventHandler CanExecuteChanged;

        public CommandHandler(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
