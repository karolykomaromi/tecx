using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Infrastructure.Commands;

namespace Infrastructure
{
    public abstract class ViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ICommandManager commandManager;

        protected ViewModel()
        {
            this.commandManager = new NullCommandManager();
        }

        public ICommandManager CommandManager
        {
            get { return commandManager; }
            set
            {
                Contract.Requires(value != null);

                commandManager = value;
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

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
                OnPropertyChanged(propertyName);
            }
        }

        #endregion Implementation of INotifyPropertyChanged

        #region Implementation of INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging = delegate { };

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
                OnPropertyChanging(propertyName);
            }
        }

        #endregion Implementation of INotifyPropertyChanging
    }
}
