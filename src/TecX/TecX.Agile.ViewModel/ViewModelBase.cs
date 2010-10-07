using System;
using System.ComponentModel;
using System.Linq.Expressions;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion Implementation of INotifyPropertyChanged

        #region Implementation of INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging = delegate { };

        #endregion Implementation of INotifyPropertyChanging

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            Guard.AssertNotNull(propertySelector, "propertySelector");

            string propertyName = GetPropertyName(propertySelector);

            PropertyChangedEventHandler temporaryHandler = PropertyChanged;
            if (temporaryHandler != null)
            {
                temporaryHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanging<T>(Expression<Func<T>> propertySelector)
        {
            Guard.AssertNotNull(propertySelector, "propertySelector");

            string propertyName = GetPropertyName(propertySelector);

            PropertyChangingEventHandler temporaryHandler = PropertyChanging;
            if (temporaryHandler != null)
            {
                temporaryHandler(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        private static string GetPropertyName<T>(Expression<Func<T>> propertySelector)
        {
            Guard.AssertNotNull(propertySelector, "propertySelector");

            MemberExpression member = propertySelector.Body as MemberExpression;

            if (member != null)
            {
                return member.Member.Name;
            }

            return string.Empty;
        }
    }
}