using System;
using System.Windows.Input;

namespace Client.Helper
{
    class ParameterizedCommandHandler : ICommand
    {
        private Action<object> action;
        private Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged;

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

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}
