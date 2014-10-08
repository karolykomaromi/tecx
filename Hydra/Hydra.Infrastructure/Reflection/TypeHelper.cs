namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;

    public static class TypeHelper
    {
        public static bool ImplementsOpenGenericInterface(Type implementationType, Type openGenericInterface)
        {
            Contract.Requires(implementationType != null);
            Contract.Requires(openGenericInterface != null);
            Contract.Requires(openGenericInterface.IsInterface);
            Contract.Requires(openGenericInterface.IsGenericType);
            Contract.Requires(openGenericInterface.IsGenericTypeDefinition);
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            bool implementsOpenGenericInterface =
                implementationType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterface);

            return implementsOpenGenericInterface;
        }

        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);
            Contract.Requires(propertySelector.Body is MemberExpression);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            MemberExpression property = (MemberExpression)propertySelector.Body;

            return property.Member.Name;
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);
            Contract.Requires(propertySelector.Body is MemberExpression);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            MemberExpression property = (MemberExpression)propertySelector.Body;

            return property.Member.Name;
        }
    }
}