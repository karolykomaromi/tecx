using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TecX.Common
{
    /// <summary>
    /// Helper classe for common operations on types
    /// </summary>
    public static class TypeHelper
    {
        #region Constants

        private static class Constants
        {
            /// <summary>
            /// Stores string to represent a <i>null</i> reference
            /// </summary>
            public const string NullString = "<null>";

            /// <summary>16</summary>
            public const int DefaultLengthPerStringFormatArg = 16;
        }

        #endregion Constants

        #region String

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

            if (args == null ||
                args.Length == 0)
            {
                return format;
            }

            try
            {
                // initialize a StringBuilder as we might have quite a few changes to make to the
                // format string and make sure that it has a reasonable initial capacity so we do not have to
                // reallocate and increase the memory reserved for the builder
                StringBuilder sb = new StringBuilder(format, format.Length + args.Length * Constants.DefaultLengthPerStringFormatArg);

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

        /// <summary>
        /// Determines the length of a <see cref="string"/>
        /// </summary>
        /// <param name="arg">The string.</param>
        /// <returns>
        /// Length of the <see cref="string"/>, <i>-1</i> if the argument is <i>null</i>
        /// </returns>
        public static int SafeLength(string arg)
        {
            return (arg == null) ? -1 : arg.Length;
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

        #endregion String

        #region Collections

        /// <summary>
        /// Checks wether the <see cref="ICollection"/> is <i>null</i> or empty
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>
        /// <i>true</i> if the collection is <i>null</i> or empty; <i>false</i> otherwise
        /// </returns>
        public static bool IsEmpty(this ICollection collection)
        {
            return (collection == null) ||
                   (collection.Count == 0);
        }

        /// <summary>
        /// Checks wether the <see cref="IEnumerable"/> is <i>null</i> or empty
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        /// <returns>
        /// <i>true</i> if the enumerable is <i>null</i> or empty; <i>false</i> otherwise
        /// </returns>
        public static bool IsEmpty(IEnumerable enumerable)
        {
            if (enumerable == null)
                return true;

            IEnumerator iter = enumerable.GetEnumerator();

            if (iter.MoveNext() == false)
                return true;

            return false;
        }

        /// <summary>
        /// Determines the number of elements in an <see cref="ICollection"/>
        /// </summary>
        /// <param name="collection">The collection</param>
        /// <returns>The number of elements in the collection; <i>-1</i> if the collection is <i>null</i></returns>
        public static int SafeCount(ICollection collection)
        {
            return (collection == null) ? -1 : collection.Count;
        }

        #endregion Collections

        #region Other types

        /// <summary>
        /// Checks wether a <see cref="Guid"/> is a <i>Guid.Empty</i>
        /// </summary>
        /// <param name="guid">The value to check</param>
        /// <returns><i>true</i> if the value is <i>Guid.Empty</i>; <i>false</i> otherwise</returns>
        public static bool IsEmpty(Guid guid)
        {
            return (guid == Guid.Empty);
        }

        /// <summary>
        /// Checks wether a <see cref="DateTime"/> is either <i>DateTime.MinValue</i>, 
        /// <i>DateTime.MaxValue</i> or <i>default(DateTime)</i>
        /// </summary>
        /// <param name="dateTime">The value to check</param>
        /// <returns><i>true</i> if the value is either <i>DateTime.MinValue</i>, 
        /// <i>DateTime.MaxValue</i> or <i>default(DateTime)</i>; <i>false</i> otherwise
        /// </returns>
        public static bool IsEmpty(DateTime dateTime)
        {
            return (dateTime == default(DateTime) ||
                   (dateTime == DateTime.MaxValue) ||
                   (dateTime == DateTime.MinValue));
        }

        /// <summary>
        /// Checks wether a string follows the Guid pattern
        /// </summary>
        /// <param name="expression">The string to check</param>
        /// <returns><i>true</i> if the string is a Guid; <i>false</i> otherwise</returns>
        public static bool IsGuid(string expression)
        {
            if (expression != null)
            {
                Regex regex = new RegexBuilder()
                    .StartingFromBeginning()
                    .AHexDigit().OccursForSpecificNumberOfTimes(8)
                    .ASpecificChar('-')
                    .AHexDigit().OccursForSpecificNumberOfTimes(4)
                    .ASpecificChar('-')
                    .AHexDigit().OccursForSpecificNumberOfTimes(4)
                    .ASpecificChar('-')
                    .AHexDigit().OccursForSpecificNumberOfTimes(4)
                    .ASpecificChar('-')
                    .AHexDigit().OccursForSpecificNumberOfTimes(12)
                    .ToEndOfString();

                return regex.IsMatch(expression);
            }
            return false;
        }

        /// <summary>
        /// Gets a hashcode from an object
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The hashcode of the object;<c>0</c> if the object is <c>null</c></returns>
        public static int GetNullSafeHashCode(object value)
        {
            return (value == null ? 0 : value.GetHashCode());
        }

        /// <summary>
        /// Gets a hashcode from a dictionary
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The dictionary.</param>
        /// <returns><c>0</c> if the dictionary is <c>null</c>; the accumulated hashcodes of the
        /// values in the dictionary otherwise</returns>
        public static int GetNullSafeHashCode<TKey, TValue>(IDictionary<TKey, TValue> value)
        {
            if (value == null)
            {
                return 0;
            }

            int hash = 0;

            foreach (TValue item in value.Values)
            {
                hash ^= GetNullSafeHashCode(item);
            }

            return hash;
        }

        /// <summary>
        /// Tries to parse a string as decimal value
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="result">The result</param>
        /// <returns><i>true</i> if the string is successfully parsed; <i>false</i> otherwise</returns>
        public static bool TryParse(string value, out decimal result)
        {
            //empty string or null cannot be parsed
            if (string.IsNullOrEmpty(value))
            {
                result = default(decimal);
                return false;
            }

            //determines the NumberDecimalSeparator
            int idxComma = value.LastIndexOf(',');
            int idxPeriod = value.LastIndexOf('.');

            if (idxComma > idxPeriod)
            {
                //german culture uses comma as decimal separator
                if (decimal.TryParse(value, NumberStyles.Float, new CultureInfo("de-DE"), out result))
                {
                    return true;
                }
            }
            else
            {
                //invariant culture uses period as decimal separator
                if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                {
                    return true;
                }
            }

            //parsing not successfull
            result = default(decimal);
            return false;
        }

        #endregion Other types

        #region IsInRange

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
        public static bool IsInRange<T>(T value, T lower, T upper)
            where T : IComparable
        {
            Guard.AssertNotNull(value, "value");
            Guard.AssertNotNull(lower, "lower");
            Guard.AssertNotNull(upper, "upper");

            int notLess = lower.CompareTo(value);
            int notGreater = upper.CompareTo(value);

            bool between = (notLess <= 0) && (notGreater >= 0);

            return between;
        }

        #endregion IsInRange
    }
}