namespace Main.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Input;
    using Infrastructure.Caching;
    using Infrastructure.Events;
    using Infrastructure.I18n;

    public class ChangeLanguageCommand : ICommand
    {
        private readonly IEventAggregator eventAggregator;

        public ChangeLanguageCommand(IEventAggregator eventAggregator)
        {
            Contract.Requires(eventAggregator != null);

            this.eventAggregator = eventAggregator;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CultureInfo culture = parameter as CultureInfo;

            if (culture != null)
            {
                this.eventAggregator.Publish(new LanguageChanging(culture));

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                // must invalidate cache before triggering reload of localized resources
                this.eventAggregator.Publish(new CacheInvalidated(CacheRegions.Resources));
                this.eventAggregator.Publish(new LanguageChanged(culture));
            }
        }
    }
}