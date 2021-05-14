using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Navigator
{
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Action<T> execute = null;
        private readonly Predicate<object> canExecute = null;

        #endregion


        #region Constructors

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion


        #region ICommand Implementation

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (parameter is T)
            {
                var typedParameter = (T)parameter;
                execute(typedParameter);
            }
        }

        #endregion
    }
}