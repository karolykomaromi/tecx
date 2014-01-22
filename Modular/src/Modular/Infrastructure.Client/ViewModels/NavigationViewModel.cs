using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace Infrastructure.ViewModels
{
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
            get { return this.destination; }

            set
            {
                if (this.destination != value)
                {
                    OnPropertyChanging(() => this.Destination);
                    this.destination = value;
                    OnPropertyChanged(() => this.Destination);
                }
            }
        }

        public string Name
        {
            get { return this.name; }

            set
            {
                if (this.name != value)
                {
                    OnPropertyChanging(() => this.Name);
                    this.name = value;
                    OnPropertyChanged(() => this.Name);
                }
            }
        }

        public ICommand NavigationCommand
        {
            get { return navigationCommand; }
        }
    }
}
