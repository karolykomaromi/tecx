namespace Infrastructure.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;

    using Infrastructure.I18n;

    public class NavigationViewModel : ViewModel
    {
        private readonly ICommand navigationCommand;
        private readonly LocalizedString name;
        private Uri destination;

        public NavigationViewModel(ICommand navigationCommand, ResxKey resourceKey)
        {
            Contract.Requires(navigationCommand != null);

            this.navigationCommand = navigationCommand;

            this.name = new LocalizedString(this, ReflectionHelper.GetPropertyName(() => this.Name), resourceKey, this.OnPropertyChanged);
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
                return this.name.Value;
            }
        }

        public ICommand NavigationCommand
        {
            get { return this.navigationCommand; }
        }
    }
}
