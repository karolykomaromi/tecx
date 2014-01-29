namespace Infrastructure.Theming
{
    using System;

    public interface IThemingManager
    {
        event EventHandler<ThemeChangedEventArgs> ThemeChanged;

        Uri CurrentTheme { get; }

        void ChangeTheme(Uri newThemeUri);
    }
}