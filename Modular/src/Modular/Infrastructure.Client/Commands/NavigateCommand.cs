namespace Infrastructure.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Regions;

    public class NavigateCommand : ICommand
    {
        private readonly INavigateAsync navigate;

        public NavigateCommand(INavigateAsync navigate)
        {
            Contract.Requires(navigate != null);

            this.navigate = navigate;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Contract.Requires(parameter != null);

            Uri destination = parameter as Uri;

            if (destination != null)
            {
                this.navigate.RequestNavigate(destination);
            }
        }
    }
}