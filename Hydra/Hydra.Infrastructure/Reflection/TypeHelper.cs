namespace Hydra.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class TypeHelper
    {
        public static bool ImplementsOpenGenericInterface(Type implementationType, Type openGenericInterface)
        {
            Contract.Requires(implementationType != null);
            Contract.Requires(openGenericInterface != null);
            Contract.Requires(openGenericInterface.IsInterface);
            Contract.Requires(openGenericInterface.IsGenericType);
            Contract.Requires(openGenericInterface.IsGenericTypeDefinition);

            bool implementsOpenGenericInterface =
                implementationType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterface);

            return implementsOpenGenericInterface;
        }

        public static bool IsClosedVersionOfOpenGeneric(Type type, Type openGenericType)
        {
            Contract.Requires(type != null);
            Contract.Requires(openGenericType != null);
            Contract.Requires(openGenericType.IsGenericType);
            Contract.Requires(openGenericType.IsGenericTypeDefinition);

            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);
            Contract.Requires(propertySelector.Body is MemberExpression);
            Contract.Ensures(Contract.Result<string>() != null);

            MemberExpression property = (MemberExpression)propertySelector.Body;

            return property.Member.Name;
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);
            Contract.Requires(propertySelector.Body is MemberExpression);
            Contract.Ensures(Contract.Result<string>() != null);

            MemberExpression property = (MemberExpression)propertySelector.Body;

            return property.Member.Name;
        }

        public static IEnumerable<Type> GetInheritanceHierarchy(Type type)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Type>>() != null);

            Type current = type;
            while (current != null)
            {
                yield return current;

                current = current.BaseType;
            }
        }

        public static PropertyInfo GetProperty<TProperty>(Expression<Func<TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);
            Contract.Ensures(Contract.Result<PropertyInfo>() != null);

            MemberExpression property = propertySelector.Body as MemberExpression;

            PropertyInfo info;
            if (property != null && (info = property.Member as PropertyInfo) != null)
            {
                return info;
            }

            return Property.Null;
        }

        public static PropertyInfo GetProperty<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            Contract.Requires(propertySelector != null);
            Contract.Ensures(Contract.Result<PropertyInfo>() != null);

            MemberExpression property = propertySelector.Body as MemberExpression;

            PropertyInfo info;
            if (property != null && (info = property.Member as PropertyInfo) != null)
            {
                return info;
            }

            return Property.Null;
        }

        public static string GetCallerMemberName([CallerMemberName] string callerMemberName = "")
        {
            return callerMemberName;
        }

        public static bool IsOpenGeneric(Type type)
        {
            Contract.Requires(type != null);

            if (!type.IsGenericTypeDefinition)
            {
                return type.ContainsGenericParameters;
            }

            return true;
        }
    }
}