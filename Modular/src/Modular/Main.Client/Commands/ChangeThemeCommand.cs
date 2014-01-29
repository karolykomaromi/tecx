namespace Main.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.Theming;

    public class ChangeThemeCommand : ICommand
    {
        private readonly IThemingManager themingManager;

        public ChangeThemeCommand(IThemingManager themingManager)
        {
            Contract.Requires(themingManager != null);

            this.themingManager = themingManager;
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
                this.themingManager.ChangeTheme(themeUri);
            }
        }
    }
}
