using System;
using System.Windows.Input;

namespace Client.Helper
{
    class ParameterizedCommandHandler : ICommand
    {
        private Action<object> action;
        private Predicate<object> canExecute;

        public ParameterizedCommandHandler (Action<object> action, Predicate<object> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
