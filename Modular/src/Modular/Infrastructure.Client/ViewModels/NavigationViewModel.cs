namespace Infrastructure.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;

    using Infrastructure.I18n;

    public class NavigationViewModel : TitledViewModel
    {
        private readonly ICommand navigationCommand;
        private Uri destination;

        public NavigationViewModel(ICommand navigationCommand, ResxKey resourceKey)
            : base(resourceKey)
        {
            Contract.Requires(navigationCommand != null);

            this.navigationCommand = navigationCommand;
        }

        public Uri Destination
        {
            get
            {
                return this.destination;
            }

            set
            {
                if (this.destination != value)
                {
                    this.OnPropertyChanging(() => this.Destination);
                    this.destination = value;
                    this.OnPropertyChanged(() => this.Destination);
                }
            }
        }

        public ICommand NavigationCommand
        {
            get { return this.navigationCommand; }
        }
    }
}
