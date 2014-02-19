namespace Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    public static class ReflectionHelper
    {
        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            MemberExpression property = (MemberExpression)propertySelector.Body;

            return property.Member.Name;
        }

        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            MemberExpression property = (MemberExpression)propertySelector.Body;

            return property.Member.Name;
        }
    }
}