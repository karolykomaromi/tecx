namespace Infrastructure.Theming
{
    using System;
    using System.Windows.Threading;
    using Infrastructure.Events;
    using Microsoft.Practices.ServiceLocation;

    public class ThemingManager : Alerting<ThemeChangedEventArgs>, IThemingManager
    {
        private static readonly Lazy<IThemingManager> Lazy = new Lazy<IThemingManager>(() => ServiceLocator.Current.GetInstance<IThemingManager>());

        public ThemingManager(Dispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public static event EventHandler<ThemeChangedEventArgs> ThemeChanged
        {
            add { Current.ThemeChanged += value; }
            remove { Current.ThemeChanged -= value; }
        }

        public static void NotifyThemeChanged(Uri newThemeUri)
        {
            Current.NotifyThemeChanged(newThemeUri);
        }

        event EventHandler<ThemeChangedEventArgs> IThemingManager.ThemeChanged
        {
            add { this.AddHandler(value); }
            remove { this.RemoveHandler(value); }
        }

        public static IThemingManager Current
        {
            get { return Lazy.Value; }
        }

        void IThemingManager.NotifyThemeChanged(Uri newThemeUri)
        {
            this.RaiseEvent(new ThemeChangedEventArgs(newThemeUri));
        }
    }
}
