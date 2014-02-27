namespace Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

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

        public static string GetDefaultNamespace(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            DefaultNamespaceAttribute attribute = assembly.GetCustomAttributes(typeof(DefaultNamespaceAttribute), false)
                                                    .OfType<DefaultNamespaceAttribute>()
                                                    .FirstOrDefault();

            if (attribute != null)
            {
                return attribute.DefaultNamespace;
            }

            string shortestNamespace = assembly.GetExportedTypes()
                                        .Select(t => t.Namespace).Distinct()
                                        .OrderBy(ns => ns.Length)
                                        .FirstOrDefault();

            if (!string.IsNullOrEmpty(shortestNamespace))
            {
                return shortestNamespace;
            }

            string msg = string.Format("Can't compute default namespace for assembly '{0}'. No DefaultNamespaceAttribute and no public types found.", assembly.FullName);
            throw new InvalidOperationException(msg);
        }
    }
}