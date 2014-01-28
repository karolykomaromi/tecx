namespace Infrastructure.Theming
{
    using System;

    public interface IThemingManager
    {
        event EventHandler<ThemeChangedEventArgs> ThemeChanged;

        void NotifyThemeChanged(Uri newThemeUri);
    }
}