namespace Infrastructure.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.I18n;

    public class NavigationViewModel : TitledViewModel
    {
        private readonly ICommand navigationCommand;
        private readonly Uri destination;

        public NavigationViewModel(ICommand navigationCommand, ResxKey resourceKey, Uri destination)
            : base(resourceKey)
        {
            Contract.Requires(navigationCommand != null);
            Contract.Requires(destination != null);

            this.navigationCommand = navigationCommand;
            this.destination = destination;
        }

        public Uri Destination
        {
            get { return this.destination; }
        }

        public ICommand NavigationCommand
        {
            get { return this.navigationCommand; }
        }
    }
}
