namespace Infrastructure.Theming
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Infrastructure.Events;
    using Microsoft.Practices.ServiceLocation;

    public class ThemingManager : Alerting<ThemeChangedEventArgs>, IThemingManager
    {
        private static readonly Lazy<IThemingManager> Lazy = new Lazy<IThemingManager>(() => ServiceLocator.Current.GetInstance<IThemingManager>());
        private Uri currentTheme;

        public ThemingManager(Dispatcher dispatcher)
            : base(dispatcher)
        {
            this.currentTheme = new Uri("/Infrastructure.Client;component/Assets/Themes/DefaultTheme.xaml", UriKind.Relative);
        }

        public static event EventHandler<ThemeChangedEventArgs> ThemeChanged
        {
            add { Current.ThemeChanged += value; }
            remove { Current.ThemeChanged -= value; }
        }

        event EventHandler<ThemeChangedEventArgs> IThemingManager.ThemeChanged
        {
            add { this.AddHandler(value); }
            remove { this.RemoveHandler(value); }
        }

        public static Uri CurrentTheme
        {
            get { return Current.CurrentTheme; }
        }

        public static IThemingManager Current
        {
            get { return Lazy.Value; }
        }

        Uri IThemingManager.CurrentTheme
        {
            get { return this.currentTheme; }
        }

        public static void ChangeTheme(Uri newThemeUri)
        {
            Current.ChangeTheme(newThemeUri);
        }

        void IThemingManager.ChangeTheme(Uri newThemeUri)
        {
            ResourceDictionary dictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source != null && t.Source.Equals(this.currentTheme));

            if (dictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(dictionary);

                ResourceDictionary newTheme = new ResourceDictionary { Source = newThemeUri };

                Application.Current.Resources.MergedDictionaries.Add(newTheme);
            }

            this.currentTheme = newThemeUri;

            this.RaiseEvent(new ThemeChangedEventArgs(newThemeUri));
        }
    }
}
