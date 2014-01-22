namespace Infrastructure.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;

    public class NavigationViewModel : ViewModel
    {
        private readonly ICommand navigationCommand;
        private Uri destination;
        private string name;

        public NavigationViewModel(ICommand navigationCommand)
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

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.OnPropertyChanging(() => this.Name);
                    this.name = value;
                    this.OnPropertyChanged(() => this.Name);
                }
            }
        }

        public ICommand NavigationCommand
        {
            get { return this.navigationCommand; }
        }
    }
}
