using System;
using System.Windows.Input;

namespace Client.Helper
{
    class ParameterizedCommandHandler : ICommand
    {
        private Action<object> action;
        private bool canExecute;

        public event EventHandler CanExecuteChanged;

        public ParameterizedCommandHandler (Action<object> action, bool canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
