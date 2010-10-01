using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion Implementation of INotifyPropertyChanged


        protected void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            Guard.AssertNotNull(propertySelector, "propertySelector");

            string propertyName = GetPropertyName(propertySelector);

            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
