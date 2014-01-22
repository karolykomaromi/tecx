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

        protected ViewModel()
        {
            this.commandManager = new NullCommandManager();
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
    }
}
