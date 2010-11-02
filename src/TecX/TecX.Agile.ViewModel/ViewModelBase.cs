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

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Customer customer = new Customer { Name = "old name" };

    //        var changing = from evt in Observable.FromEvent<PropertyChangingEventArgs>(customer, "PropertyChanging")
    //                       select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

    //        var changed = from evt in Observable.FromEvent<PropertyChangedEventArgs>(customer, "PropertyChanged")
    //                      select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

    //        var beforeAfter = changing
    //                            .CombineLatest(changed, (before, after) => new { Before = before.Value, After = after.Value })
    //                            .Where(ba => ba.After != ba.Before);

    //        beforeAfter.Subscribe(x => Console.WriteLine(x.Before + " " + x.After));

    //        for (int i = 0; i < 1000; i++)
    //        {
    //            customer.Name = i.ToString();
    //        }
    //    }
    //}

    //public class Customer : INotifyPropertyChanging, INotifyPropertyChanged
    //{
    //    private string _name;

    //    public string Name
    //    {
    //        get { return _name; }
    //        set
    //        {
    //            if (value == _name)
    //                return;

    //            OnPropertyChanging(() => Name);
    //            _name = value;
    //            OnPropertyChanged(() => Name);
    //        }
    //    }

    //    public event PropertyChangingEventHandler PropertyChanging;
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void OnPropertyChanging<T>(Expression<Func<T>> expression)
    //    {
    //        MemberExpression memberSelector = (MemberExpression)expression.Body;

    //        if (PropertyChanging != null)
    //        {
    //            PropertyChanging(this, new PropertyChangingEventArgs(memberSelector.Member.Name));
    //        }
    //    }

    //    private void OnPropertyChanged<T>(Expression<Func<T>> expression)
    //    {
    //        MemberExpression memberSelector = (MemberExpression)expression.Body;

    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(memberSelector.Member.Name));
    //        }
    //    }
    //}
}