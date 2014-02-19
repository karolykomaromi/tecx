namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using Infrastructure.Events;
    using Infrastructure.Reflection;

    public class LocalizedString : ISubscribeTo<LanguageChanged>
    {
        private readonly string propertyName;
        private readonly Func<string> getResource;
        private readonly Action<string> notifyPropertyChanged;

        public LocalizedString(Expression<Func<string>> propertySelector, Func<string> getResource, Action<string> notifyPropertyChanged)
            : this(propertySelector, getResource, notifyPropertyChanged, EventAggregator.Current)
        {
        }

        public LocalizedString(Expression<Func<string>> propertySelector, Func<string> getResource, Action<string> notifyPropertyChanged, IEventAggregator eventAggregator)
            : this(ReflectionHelper.GetPropertyName(propertySelector), getResource, notifyPropertyChanged, eventAggregator)
        {
        }

        public LocalizedString(string propertyName, Func<string> getResource, Action<string> notifyPropertyChanged)
            : this(propertyName, getResource, notifyPropertyChanged, EventAggregator.Current)
        {
        }

        public LocalizedString(string propertyName, Func<string> getResource, Action<string> notifyPropertyChanged, IEventAggregator eventAggregator)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));
            Contract.Requires(getResource != null);
            Contract.Requires(notifyPropertyChanged != null);
            Contract.Requires(eventAggregator != null);

            this.propertyName = propertyName;
            this.getResource = getResource;
            this.notifyPropertyChanged = notifyPropertyChanged;

            eventAggregator.Subscribe(this);
        }

        public string Value
        {
            get { return this.getResource(); }
        }

        public void Handle(LanguageChanged message)
        {
            this.notifyPropertyChanged(this.propertyName);
        }
    }
}