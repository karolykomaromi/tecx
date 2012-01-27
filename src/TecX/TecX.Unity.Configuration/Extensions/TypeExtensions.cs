namespace TecX.Unity.Configuration.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;
    using TecX.Common.Extensions.Error;

    public static class TypeExtensions
    {
        public static PropertyInfo[] PublicProperties(this object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public static IEnumerable<PropertyInfo> OfString(this IEnumerable<PropertyInfo> properties)
        {
            Guard.AssertNotNull(properties, "properties");

            return properties.Where(p => p.PropertyType == typeof(string));
        }

        public static IEnumerable<PropertyInfo> NotOfString(this IEnumerable<PropertyInfo> properties)
        {
            Guard.AssertNotNull(properties, "properties");

            return properties.Where(p => p.PropertyType != typeof(string));
        }

        public static ConstructorInfo MostGreedyPublicCtor(this Type type)
        {
            Guard.AssertNotNull(type, " type");

            var ctors =
                type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).OrderByDescending(
                    c => c.GetParameters().Length).ToArray();

            if (ctors.Length == 0)
            {
                throw new ArgumentException("No public constructor found.", "type").WithAdditionalInfo("type", type);
            }

            if (ctors.Length == 1)
            {
                return ctors[0];
            }

            if (ctors[0].GetParameters().Length == ctors[1].GetParameters().Length)
            {
                throw new ArgumentException("Multiple constructors with same number of parameters. Cannot disambiguate.");
            }

            return ctors[0];
        }

        public static ParameterInfo FindParameterNamed(this ConstructorInfo ctor, string parameterName)
        {
            Guard.AssertNotNull(ctor, "ctor");
            Guard.AssertNotEmpty(parameterName, "parameterName");

            var parameter = ctor.GetParameters().SingleOrDefault(p => p.Name == parameterName);

            if (parameter != null)
            {
                return parameter;
            }

            throw new ArgumentException("No parameter with given name could be found.", "parameterName");
        }
    }
}
