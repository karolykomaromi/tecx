namespace TecX.Common
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    using TecX.Common.Extensions.Error;

    using ArgumentException = TecX.Common.Error.ArgumentException;
    using ArgumentNullException = TecX.Common.Error.ArgumentNullException;
    using ArgumentOutOfRangeException = TecX.Common.Error.ArgumentOutOfRangeException;

    /// <summary>
    /// Helper class for common method parameter validation tasks.
    /// </summary>
    public static class Guard
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            /// <summary>ParameterNull '{0}' should not be NULL!</summary>
            public const string ParameterNull = "Parameter '{0}' must not be NULL!";

            /// <summary>parameter '{0}' should not be empty!</summary>
            public const string ParameterEmpty = "Parameter '{0}' must not be empty!";

            /// <summary>Parameter '{0}' with a value of '{1}' is not between '{2}' and '{3}!</summary>
            public const string ParameterNotInRange =
                "Parameter '{0}' with a value of '{1}' is not between '{2}' and '{3}!";

            /// <summary>&lt;no parameter name&gt;</summary>
            public const string NoParameterName = "<no parameter name>";
        }

        [DebuggerStepThrough]
        public static void AssertNotNull(object param, string paramName)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotNull<T>(Expression<Func<T>> selector)
        {
            var memberSelector = (MemberExpression)selector.Body;
            var constantSelector = (ConstantExpression)memberSelector.Expression;
            object value = ((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);

            if (value == null)
            {
                string name = ((MemberExpression)selector.Body).Member.Name;
                throw new ArgumentNullException(name);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotEmpty(string param, string paramName)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException(Constants.ParameterEmpty, paramName);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotEmpty(Expression<Func<string>> selector)
        {
            var memberSelector = (MemberExpression)selector.Body;
            var constantSelector = (ConstantExpression)memberSelector.Expression;
            string value = (string)((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);

            if (value == null)
            {
                string paramName = ((MemberExpression)selector.Body).Member.Name;
                throw new ArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(value))
            {
                string paramName = ((MemberExpression)selector.Body).Member.Name;
                throw new ArgumentException("String must not be empty.", paramName);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                throw new ArgumentNullException(paramName, Constants.ParameterNull);
            }

            if (param.Count == 0)
            {
                throw new ArgumentException(paramName, Constants.ParameterEmpty).WithAdditionalInfo("param", param);
            }
        }

        [DebuggerStepThrough]
        public static void AssertInRange<T>(T param, string paramName, T min, T max) where T : IComparable
        {
            AssertNotNull(param, "param");
            AssertNotNull(min, "min");
            AssertNotNull(max, "max");

            paramName = paramName ?? Constants.NoParameterName;

            if (param.CompareTo(min) < 0 || param.CompareTo(max) > 0)
            {
                string message = string.Format(Constants.ParameterNotInRange, paramName, param, min, max);
                throw new ArgumentOutOfRangeException(paramName, string.Format(message));
            }
        }
    }
}