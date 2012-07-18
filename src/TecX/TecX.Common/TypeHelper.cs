namespace TecX.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Text.RegularExpressions;

    using TecX.Common.Reflection;

    /// <summary>
    /// Helper classe for common operations on types
    /// </summary>
    public static class TypeHelper
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            /// <summary>16</summary>
            public const int DefaultLengthPerStringFormatArg = 16;
        }

        /// <summary>
        /// Performs a safe string format operation
        /// </summary>
        /// <param name="format">The format string</param>
        /// <param name="args">The arguments</param>
        /// <returns>The formatted string; the original format string if an error occurs</returns>
        public static string SafeFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                // if the format string is already empty,
                // then just return an empty string
                return string.Empty;
            }

            try
            {
                // if there are no arguments to put into the format string
                // then just return the format string as is
                if (args == null || args.Length == 0)
                {
                    return format;
                }

                string result = string.Format(format, args);

                return result;
            }
            catch (FormatException)
            {
                // if an error occured fallback to the most complicated
                // and slowest way to fill the arguments into the format string
                return SafeFormatIntern(format, args);
            }
        }

        /// <summary>
        /// Asserts that a given string is not <i>null</i>
        /// </summary>
        /// <param name="arg">A string.</param>
        /// <returns>
        /// The argument string or <i>string.Empty</i> if the original string was <i>null</i>.
        /// </returns>
        public static string ToNullSafeString(string arg)
        {
            return ToNullSafeString(arg, string.Empty);
        }

        /// <summary>
        /// Asserts that a given string is not <i>null</i>.
        /// </summary>
        /// <param name="def">The default value if the original string is <i>null</i>.</param>
        /// <param name="arg">The string.</param>
        /// <returns>
        /// The argument string, or the default string, if the argument is <i>null</i>.
        /// </returns>
        public static string ToNullSafeString(string arg, string def)
        {
            def = def ?? string.Empty;

            return arg ?? def;
        }

        /// <summary>
        /// Performs a safe <i>object.ToString()</i> operation
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>
        /// <i>obj.ToString()</i> if the object is not <i>null</i>; <i>string.Empty</i> otherwise
        /// </returns>
        public static string ToNullSafeString(object obj)
        {
            return ToNullSafeString(obj, string.Empty);
        }

        /// <summary>
        /// Performs a safe <i>object.ToString()</i> operation
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="def">The default value if the object is <i>null</i>.</param>
        /// <returns>
        /// <i>obj.ToString()</i> if the object is not <i>null</i>; <i>string.Empty</i> otherwise
        /// </returns>
        public static string ToNullSafeString(object obj, string def)
        {
            def = def ?? string.Empty;

            return (obj == null) ? def : obj.ToString();
        }

        /// <summary>
        /// Checks wether a value is inside a range (including the upper and lower bound of the range)
        /// </summary>
        /// <typeparam name="T">
        /// Type of values to compare
        /// </typeparam>
        /// <param name="value">
        /// The value to check
        /// </param>
        /// <param name="lower">
        /// The lower bound of the range
        /// </param>
        /// <param name="upper">
        /// The upper bound of the range
        /// </param>
        /// <returns>
        /// <i>true</i> if (lower &lt;= value &lt; upper); <i>false</i> otherwise
        /// </returns>
        public static bool IsInRange<T>(T value, T lower, T upper) where T : IComparable
        {
            Guard.AssertNotNull(value, "value");
            Guard.AssertNotNull(lower, "lower");
            Guard.AssertNotNull(upper, "upper");

            int notLess = lower.CompareTo(value);
            int notGreater = upper.CompareTo(value);

            bool between = (notLess <= 0) && (notGreater >= 0);

            return between;
        }

        public static bool IsInNamespace(this Type type, string nameSpace)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotEmpty(nameSpace, "nameSpace");

            return type.Namespace.StartsWith(nameSpace);
        }

        public static IEnumerable<Type> AllInterfaces(this Type type)
        {
            foreach (Type @interface in type.GetInterfaces())
            {
                yield return @interface;
            }
        }

        public static bool IsConcrete(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool CanBeCreated(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            return type.IsConcrete() && Constructor.HasConstructors(type);
        }

        public static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null)
            {
                return false;
            }

            if (pluggedType.IsInterface || pluggedType.IsAbstract)
            {
                return false;
            }

            if (pluginType.IsOpenGeneric())
            {
                return GenericsHelper.CanBeCast(pluginType, pluggedType);
            }

            if (IsOpenGeneric(pluggedType))
            {
                return false;
            }

            return pluginType.IsAssignableFrom(pluggedType);
        }

        public static bool IsOpenGeneric(this Type type)
        {
            Guard.AssertNotNull(type, "type");

            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        /// <summary>
        /// Parses the format string using regular expressions and replaces the parameters
        /// one after another
        /// </summary>
        /// <param name="format">The format string</param>
        /// <param name="args">The arguments</param>
        /// <returns>Either the format string (empty string if format is NULL), or the format
        /// string with the placeholders replaced by the arguments</returns>
        private static string SafeFormatIntern(string format, params object[] args)
        {
            format = ToNullSafeString(format);

            if (args == null || args.Length == 0)
            {
                return format;
            }

            try
            {
                // initialize a StringBuilder as we might have quite a few changes to make to the
                // format string and make sure that it has a reasonable initial capacity so we do not have to
                // reallocate and increase the memory reserved for the builder
                StringBuilder sb = new StringBuilder(
                    format,
                    format.Length + (args.Length * Constants.DefaultLengthPerStringFormatArg));

                Regex regex = new Regex(@"{\d+}");
                var matches = regex.Matches(format);

                int maxCount = Math.Min(matches.Count, args.Length);

                for (int i = 0; i < maxCount; i++)
                {
                    sb = sb.Replace(matches[i].Value, ToNullSafeString(args[i]));
                }

                return sb.ToString();
            }
            catch (Exception)
            {
                return format;
            }
        }
    }
}