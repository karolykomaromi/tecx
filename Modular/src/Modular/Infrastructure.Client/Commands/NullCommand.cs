namespace Infrastructure.Commands
{
    using System;
    using System.Windows.Input;

    public class NullCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}