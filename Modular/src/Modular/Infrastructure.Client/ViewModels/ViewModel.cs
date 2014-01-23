using Infrastructure.I18n;

namespace Infrastructure.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using Infrastructure.Commands;

    public abstract class ViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ICommandManager commandManager;
        private IResourceManager resourceManager;

        protected ViewModel()
        {
            this.commandManager = new NullCommandManager();
            this.resourceManager = new EchoResourceManager();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event PropertyChangingEventHandler PropertyChanging = delegate { };

        public ICommandManager CommandManager
        {
            get
            {
                return this.commandManager;
            }

            set
            {
                Contract.Requires(value != null);

                this.commandManager = value;
            }
        }

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            this.CommandManager.InvalidateRequerySuggested();
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

            MemberExpression property = (MemberExpression)propertySelector.Body;

            string propertyName = property.Member.Name;
            string typeName = this.GetType().FullName;

            string key = typeName + "." + propertyName;

            return this.ResourceManager[key];
        }
    }
}
