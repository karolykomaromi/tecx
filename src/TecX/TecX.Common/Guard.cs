namespace TecX.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;

    using TecX.Common.Extensions.Error;

    /// <summary>
    /// Helper class for common method paramter validation tasks.
    /// </summary>
    public static class Guard
    {
        #region Constants

        private static class Constants
        {
            public static class Messages
            {
                /// <summary>paramument '{0}' should not be NULL!</summary>
                public const string paramumentNull = "paramument '{0}' must not be NULL!";

                /// <summary>paramument '{0}' should not be empty!</summary>
                public const string paramumentEmpty = "paramument '{0}' must not be empty!";

                /// <summary>Condition not met!</summary>
                public const string ConditionNotMet = "Condition not met!";

                /// <summary>Invalid switch value '{0}' for parameter '{1}'.</summary>
                public const string InvalidSwitchValue = "Invalid switch value '{0}' for parameter '{1}'.";

                /// <summary>paramument '{0}' with a value of '{1}' is not between '{2}' and '{3}!</summary>
                public const string paramumentNotInRange =
                    "paramument '{0}' with a value of '{1}' is not between '{2}' and '{3}!";

                /// <summary>&lt;no parameter name&gt;</summary>
                public const string NoParamName = "<no parameter name>";

                /// <summary>Parameter {0} is not of Type {1}</summary>
                public const string WrongType = "Parameter {0} is not of Type {1}";
            }
        }

        #endregion Constants

        #region AssertNotNull

        /// <summary>
        /// Asserts that the paramument is not <i>null</i>
        /// </summary>
        /// <param name="param">The parameter to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        [DebuggerStepThrough]
        public static void AssertNotNull(object param, string paramName)
        {
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
            object value = ((FieldInfo)memberSelector.Member)
                .GetValue(constantSelector.Value);

            if (value == null)
            {
                string name = ((MemberExpression)selector.Body).Member.Name;
                throw new ArgumentNullException(name);
            }
        }

        #endregion AssertNotNull

        #region AssertNotEmpty (string)

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is <see cref="string.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(string param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException(Constants.Messages.paramumentEmpty, paramName);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotEmpty(Expression<Func<string>> selector)
        {
            var memberSelector = (MemberExpression)selector.Body;
            var constantSelector = (ConstantExpression)memberSelector.Expression;
            string value = (string)((FieldInfo)memberSelector.Member)
                .GetValue(constantSelector.Value);

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

        #endregion AssertNotEmpty (string)

        #region AssertNotEmpty (ICollection)

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> contains no elements</exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName)
        {
            AssertNotEmpty(param, paramName, Constants.Messages.paramumentNull, Constants.Messages.paramumentEmpty);
        }

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> contains no elements</exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName, string message)
        {
            AssertNotEmpty(param, paramName, message, message);
        }

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <param name="format">The format string for the error message.</param>
        /// <param name="args">The parameters for the error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> contains no elements</exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName, string format, params object[] args)
        {
            AssertNotEmpty(param, paramName, TypeHelper.SafeFormat(format, args));
        }

        #endregion AssertNotEmpty (ICollection)

        #region AssertNotEmpty (Guid)

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is <see cref="Guid.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Guid param, string paramName)
        {
            AssertNotEmpty(param, paramName, Constants.Messages.paramumentEmpty);
        }

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is <see cref="Guid.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Guid param, string paramName, string message)
        {
            if (param == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                                                      TypeHelper.ToNullSafeString(message)).WithAdditionalInfo("param", param);
            }
        }

        /// <summary>
        /// Asserts that the paramument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <param name="format">The format string for the error message.</param>
        /// <param name="args">The parameters for the error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is <see cref="Guid.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Guid param, string paramName, string format, params object[] args)
        {
            AssertNotEmpty(param, paramName, TypeHelper.SafeFormat(format, args));
        }

        #endregion AssertNotEmpty (Guid)

        #region AssertCondition

        /// <summary>
        /// Asserts that a condition is met.
        /// </summary>
        /// <param name="condition">The condition to be met</param>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <param name="format">The format string for the error message.</param>
        /// <param name="args">The parameters for the error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> does not satisfy the <paramref name="condition"/></exception>
        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object param, string paramName, string format,
                                           params object[] args)
        {
            AssertCondition(condition, param, paramName, TypeHelper.SafeFormat(format, args));
        }

        /// <summary>
        /// Asserts that a condition is met.
        /// </summary>
        /// <param name="condition">The condition to be met</param>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> does not satisfy the <paramref name="condition"/></exception>
        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object param, string paramName, string message)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                                                      TypeHelper.ToNullSafeString(message)).WithAdditionalInfos(
                                                          new Dictionary<object, object> { { "param", param } });
            }
        }

        /// <summary>
        /// Asserts that a condition is met.
        /// </summary>
        /// <param name="condition">The condition to be met</param>
        /// <param name="param">The paramument to validate.</param>
        /// <param name="paramName">The name of the paramument (parameter in a method signature).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> does not satisfy the <paramref name="condition"/></exception>
        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object param, string paramName)
        {
            AssertCondition(condition, param, paramName, Constants.Messages.ConditionNotMet);
        }

        #endregion AssertCondition

        #region InvalidSwitchValue

        /// <summary>
        /// Can be used to throw an exception if a value in a switch-case-statement is not valid.
        /// </summary>
        /// <param name="paramName">The name of the paramument for the switch-case-statement.</param>
        /// <param name="param">The actual value of the paramument.</param>
        /// <exception cref="InvalidOperationException">Thrown when a value in a switch-case-statement
        /// is not valid</exception>
        [DebuggerStepThrough]
        public static void InvalidSwitchValue(object param, string paramName)
        {
            throw new InvalidOperationException(
                TypeHelper.SafeFormat(Constants.Messages.InvalidSwitchValue,
                                      TypeHelper.ToNullSafeString(param),
                                      TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName)));
        }

        #endregion InvalidSwitchValue

        #region AssertIsInRange

        /// <summary>
        /// Asserts that a parameter lies within a defined range
        /// </summary>
        /// <typeparam name="T">The object must be an <see cref="IComparable"/></typeparam>
        /// <param name="param">The parameter to check</param>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="min">param has to be greater than or equal to min</param>
        /// <param name="max">param has to be less than or equal to max</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters
        /// <paramref name="param"/>, <paramref name="min"/> or <paramref name="max"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is not between
        /// <paramref name="min"/> and <paramref name="max"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T param, string paramName, T min, T max)
            where T : IComparable
        {
            AssertIsInRange(param, paramName, min, max, Constants.Messages.paramumentNotInRange, paramName, param, min, max);
        }

        /// <summary>
        /// Asserts that a parameter lies within a defined range
        /// </summary>
        /// <typeparam name="T">The object must be an <see cref="IComparable"/></typeparam>
        /// <param name="param">The parameter to check</param>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="min">param has to be greater than or equal to min</param>
        /// <param name="max">param has to be less than or equal to max</param>
        /// <param name="message">The custom error message</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters
        /// <paramref name="param"/>, <paramref name="min"/> or <paramref name="max"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is not between
        /// <paramref name="min"/> and <paramref name="max"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T param, string paramName, T min, T max, string message)
            where T : IComparable
        {
            AssertNotNull(param, "param");
            AssertNotNull(min, "min");
            AssertNotNull(max, "max");

            if (!TypeHelper.IsInRange(param, min, max))
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                                                      TypeHelper.ToNullSafeString(message)).WithAdditionalInfos(
                                                          new Dictionary<object, object> { { "param", param } });
            }
        }

        /// <summary>
        /// Asserts that a parameter lies within a defined range
        /// </summary>
        /// <typeparam name="T">The object must be an <see cref="IComparable"/></typeparam>
        /// <param name="param">The parameter to check</param>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="min">param has to be greater than or equal to min</param>
        /// <param name="max">param has to be less than or equal to max</param>
        /// <param name="format">The format string for the custom error message</param>
        /// <param name="args">The paramuments for the custom error message</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters
        /// <paramref name="param"/>, <paramref name="min"/> or <paramref name="max"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> is not between
        /// <paramref name="min"/> and <paramref name="max"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T param, string paramName, T min, T max, string format, params object[] args)
            where T : IComparable
        {
            AssertIsInRange(param, paramName, min, max, TypeHelper.SafeFormat(format, args));
        }

        #endregion AssertIsInRange

        #region AssertIsType

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <typeparamref name="TTparamet"/>
        /// </summary>
        /// <typeparam name="TTparamet">The <see cref="Type"/> that must be matched by <paramref name="value"/></typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <typeparamref name="TTparamet"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType<TTparamet>(object value, string paramName)
        {
            AssertIsType<TTparamet>(value, paramName, Constants.Messages.WrongType, paramName, typeof(TTparamet).FullName);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <typeparamref name="TTparamet"/>
        /// </summary>
        /// <typeparam name="TTparamet">The <see cref="Type"/> that must be matched by <paramref name="value"/></typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The error message</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <typeparamref name="TTparamet"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType<TTparamet>(object value, string paramName, string message)
        {
            Type tparametType = typeof(TTparamet);

            AssertIsType(tparametType, value, paramName, message);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <typeparamref name="TTparamet"/>
        /// </summary>
        /// <typeparam name="TTparamet">The <see cref="Type"/> that must be matched by <paramref name="value"/></typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="format">The format string for the error message</param>
        /// <param name="args">The arguments to put in the format string</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <typeparamref name="TTparamet"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType<TTparamet>(object value, string paramName, string format, params object[] args)
        {
            string message = TypeHelper.SafeFormat(format, args);

            AssertIsType<TTparamet>(value, paramName, message);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <paramref name="tparametType"/>
        /// </summary>
        /// <param name="tparametType">The <see cref="Type"/> that must be matched by <paramref name="value"/></param>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tparametType"/> or <paramref name="value"/>
        /// are <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <paramref name="tparametType"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType(Type tparametType, object value, string paramName)
        {
            string tparametTypeFullName = (tparametType == null ? string.Empty : tparametType.FullName);

            AssertIsType(tparametType, value, paramName, Constants.Messages.WrongType, paramName, tparametTypeFullName);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <paramref name="tparametType"/>
        /// </summary>
        /// <param name="tparametType">The <see cref="Type"/> that must be matched by <paramref name="value"/></param>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="format">The format string for the error message</param>
        /// <param name="args">The paramuments to put in the format string</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tparametType"/> or <paramref name="value"/>
        /// are <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <paramref name="tparametType"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType(Type tparametType, object value, string paramName, string format,
                                        params object[] args)
        {
            AssertIsType(tparametType, value, paramName, TypeHelper.SafeFormat(format, args));
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <paramref name="tparametType"/>
        /// </summary>
        /// <param name="tparametType">The <see cref="Type"/> that must be matched by <paramref name="value"/></param>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The error message</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tparametType"/> or <paramref name="value"/>
        /// are <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <paramref name="tparametType"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType(Type tparametType, object value, string paramName, string message)
        {
            AssertNotNull(tparametType, "tparametType");
            AssertNotNull(value, "value");

            Type sourceType = value.GetType();

            if (string.IsNullOrEmpty(message))
            {
                message = TypeHelper.SafeFormat(Constants.Messages.WrongType, paramName, tparametType.FullName);
            }

            if (!tparametType.IsAssignableFrom(sourceType))
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName), message)
                    .WithAdditionalInfos(new Dictionary<object, object>
                                             {
                                                 {"tparametType", tparametType},
                                                 {"sourceType", sourceType},
                                                 {"value", value},
                                                 {"paramName", paramName}
                                             });
            }
        }

        #endregion AssertIsType

        #region Private Methods

        [DebuggerStepThrough]
        private static void AssertNotEmpty(ICollection param, string paramName, string messageNull, string messageEmpty)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName, messageNull);
            }

            if (param.Count == 0)
            {
                throw new ArgumentOutOfRangeException(paramName, messageEmpty)
                    .WithAdditionalInfo("param", param);
            }
        }

        #endregion Private Methods
    }
}