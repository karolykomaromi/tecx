namespace Infrastructure.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Regions;

    public class NavigationCommand : ICommand
    {
        private readonly INavigateAsync navigate;

        public NavigationCommand(INavigateAsync navigate)
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