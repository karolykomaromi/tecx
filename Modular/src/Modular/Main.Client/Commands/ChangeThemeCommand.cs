using System.Linq;
using System.Windows;
using Infrastructure.Events;

namespace Main.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Theming;

    public class ChangeThemeCommand : ICommand
    {
        private readonly IEventAggregator eventAggregator;

        public ChangeThemeCommand(IEventAggregator eventAggregator)
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
            Uri themeUri = parameter as Uri;

            if (themeUri != null)
            {
                ResourceDictionary dictionary =
                    Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                        theme =>
                        theme.Source != null &&
                        theme.Source.ToString().EndsWith("Theme.xaml", StringComparison.OrdinalIgnoreCase));

                if (dictionary != null)
                {
                    dictionary.Source = themeUri;
                }

                this.eventAggregator.Publish(new ThemeChanged(themeUri));
            }
        }
    }
}
