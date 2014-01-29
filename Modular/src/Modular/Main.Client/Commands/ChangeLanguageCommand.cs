namespace Main.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Windows.Input;

    using Infrastructure.Caching;
    using Infrastructure.I18n;

    public class ChangeLanguageCommand : ICommand
    {
        private readonly ILanguageManager languageManager;

        private readonly ICacheInvalidationManager cacheInvalidationManager;

        public ChangeLanguageCommand(ILanguageManager languageManager, ICacheInvalidationManager cacheInvalidationManager)
        {
            Contract.Requires(languageManager != null);
            Contract.Requires(cacheInvalidationManager != null);

            this.languageManager = languageManager;
            this.cacheInvalidationManager = cacheInvalidationManager;
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
                // must invalidate cache before triggering reload of localized resources
                this.cacheInvalidationManager.RequestInvalidate(CacheRegions.Resources);
                this.languageManager.ChangeLanguage(culture);
            }
        }
    }
}