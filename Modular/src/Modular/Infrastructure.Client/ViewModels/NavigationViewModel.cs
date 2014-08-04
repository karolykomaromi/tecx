namespace Infrastructure.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class NavigationViewModel : TitledViewModel, ISubscribeTo<IOptionsChanged<IOptions>>
    {
        private readonly LocalizedString title;
        private readonly ICommand navigationCommand;
        private readonly Uri destination;
        private readonly Action<IOptionsChanged<IOptions>, NavigationViewModel> handleOptionsChanged;

        public NavigationViewModel(ICommand navigationCommand, ResourceAccessor title, Uri destination, Action<IOptionsChanged<IOptions>, NavigationViewModel> handleOptionsChanged)
        {
            Contract.Requires(navigationCommand != null);
            Contract.Requires(destination != null);

            this.title = new LocalizedString(() => this.Title, title.GetResource, this.OnPropertyChanged);
            this.navigationCommand = navigationCommand;
            this.destination = destination;
            this.handleOptionsChanged = handleOptionsChanged ?? ((msg, vm) => { });
        }

        public override string Title
        {
            get { return this.title.Value; }
        }

        public Uri Destination
        {
            get { return this.destination; }
        }

        public ICommand NavigationCommand
        {
            get { return this.navigationCommand; }
        }

        public void Handle(IOptionsChanged<IOptions> message)
        {
            this.handleOptionsChanged(message, this);
        }
    }
}
