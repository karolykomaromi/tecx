namespace Infrastructure.I18n
{
    using System;
    using System.Windows.Threading;
    using Infrastructure.Events;
    using Microsoft.Practices.ServiceLocation;

    public class LanguageManager : Alerting, ILanguageManager
    {
        private static readonly Lazy<ILanguageManager> Lazy = new Lazy<ILanguageManager>(() => ServiceLocator.Current.GetInstance<ILanguageManager>());

        public LanguageManager(Dispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public static event EventHandler LanguageChanged
        {
            add
            {
                Current.LanguageChanged += value;
            }

            remove
            {
                Current.LanguageChanged -= value;
            }
        }

        event EventHandler ILanguageManager.LanguageChanged
        {
            add
            {
                this.AddHandler(value);
            }

            remove
            {
                this.RemoveHandler(value);
            }
        }

        public static ILanguageManager Current
        {
            get
            {
                return Lazy.Value;
            }
        }

        public static void NotifyApplicationLanguageChanged()
        {
            Current.NotifyApplicationLanguageChanged();
        }

        void ILanguageManager.NotifyApplicationLanguageChanged()
        {
            this.RaiseEvent();
        }
    }
}