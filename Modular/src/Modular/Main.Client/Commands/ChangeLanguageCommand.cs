namespace Main.Commands
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Input;

    using Infrastructure.I18n;

    public class ChangeLanguageCommand : ICommand
    {
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
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                LanguageManager.NotifyApplicationLanguageChanged();
            }
        }
    }
}