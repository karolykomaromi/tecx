namespace TecX.TestTools
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    using TecX.Common;

    public static class NotifyPropertyChanged
    {
        public static PropertyChangedExpectation<T> ShouldNotifyOn<T, TProperty>(this T owner, Expression<Func<T, TProperty>> propertyPicker)
            where T : INotifyPropertyChanged
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(propertyPicker, "propertyPicker");

            return CreateExpectation(owner, propertyPicker, true);
        }

        public static PropertyChangedExpectation<T> ShouldNotNotifyOn<T, TProperty>(this T owner, Expression<Func<T, TProperty>> propertyPicker)
            where T : INotifyPropertyChanged
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(propertyPicker, "propertyPicker");

            return CreateExpectation(owner, propertyPicker, false);
        }

        private static PropertyChangedExpectation<T> CreateExpectation<T, TProperty>(T owner, Expression<Func<T, TProperty>> pickProperty, bool eventExpected)
            where T : INotifyPropertyChanged
        {
            string propertyName = ((MemberExpression)pickProperty.Body).Member.Name;

            return new PropertyChangedExpectation<T>(owner, propertyName, eventExpected);
        }
    }
}