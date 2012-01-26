namespace TecX.Unity.Configuration.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    public static class TypeExtensions
    {
        public static PropertyInfo[] PublicProperties(this object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static PropertyInfo[] OfString(this IEnumerable<PropertyInfo> properties)
        {
            Guard.AssertNotNull(properties, "properties");

            return properties.Where(p => p.PropertyType == typeof(string)).ToArray();
        }

        public static ConstructorInfo[] PublicCtors(this Type type)
        {
            Guard.AssertNotNull(type, " type");

            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        }

        public static IEnumerable<ConstructorInfo> WithParameterNamed(this IEnumerable<ConstructorInfo> ctors, string parameterName)
        {
            Guard.AssertNotNull(ctors, "ctors");
            Guard.AssertNotEmpty(parameterName, "parameterName");

            foreach (var ctor in ctors)
            {
                if (ctor.GetParameters().Any(p => p.Name == parameterName))
                {
                    yield return ctor;
                }
            }
        }

        public static ParameterInfo GetParameter(this ConstructorInfo ctor, string name)
        {
            Guard.AssertNotNull(ctor, "ctor");
            Guard.AssertNotEmpty(name, "name");

            return ctor.GetParameters().FirstOrDefault(p => p.Name == name);
        }
    }
}
