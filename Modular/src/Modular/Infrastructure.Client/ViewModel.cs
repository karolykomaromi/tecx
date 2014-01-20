using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Infrastructure.Client
{

    public abstract class ViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
