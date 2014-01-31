namespace Infrastructure.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Text;
    using Infrastructure.Commands;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.Meta;

    public abstract class ViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private IResourceManager resourceManager;
        private IEventAggregator eventAggregator;

        protected ViewModel()
        {
            this.resourceManager = new EchoResourceManager();
            this.eventAggregator = new NullEventAggregator();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event PropertyChangingEventHandler PropertyChanging = delegate { };

        [PropertyMeta(IsListViewRelevant = false)]
        public IResourceManager ResourceManager
        {
            get
            {
                return this.resourceManager;
            }

            set
            {
                Contract.Requires(value != null);
                this.resourceManager = value;
            }
        }

        [PropertyMeta(IsListViewRelevant = false)]
        public IEventAggregator EventAggregator
        {
            get
            {
                return eventAggregator;
            }

            set
            {
                Contract.Requires(value != null);
                eventAggregator = value;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            this.EventAggregator.Publish(new CanExecuteChanged());
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            MemberExpression property = propertySelector.Body as MemberExpression;

            if (property != null)
            {
                string propertyName = property.Member.Name;
                this.OnPropertyChanged(propertyName);
            }
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging<T>(Expression<Func<T>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            MemberExpression property = propertySelector.Body as MemberExpression;

            if (property != null)
            {
                string propertyName = property.Member.Name;
                this.OnPropertyChanging(propertyName);
            }
        }

        protected virtual string Translate<T>(Expression<Func<T>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            string propertyName = ReflectionHelper.GetPropertyName(propertySelector);

            string key = new StringBuilder(this.GetType().FullName)
                .Append(".")
                .Append(propertyName)
                .Replace(".ViewModels", string.Empty)
                .Replace("ViewModel", string.Empty)
                .Replace("Label", "Label_")
                .ToString();

            return this.ResourceManager[new ResxKey(key)];
        }
    }
}
