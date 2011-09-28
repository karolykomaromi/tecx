using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

using TecX.Common.Extensions.Error;

namespace TecX.Common
{
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
                /// <summary>Argument '{0}' should not be NULL!</summary>
                public const string ArgumentNull = "Argument '{0}' must not be NULL!";

                /// <summary>Argument '{0}' should not be empty!</summary>
                public const string ArgumentEmpty = "Argument '{0}' must not be empty!";

                /// <summary>Condition not met!</summary>
                public const string ConditionNotMet = "Condition not met!";

                /// <summary>Invalid switch value '{0}' for parameter '{1}'.</summary>
                public const string InvalidSwitchValue = "Invalid switch value '{0}' for parameter '{1}'.";

                /// <summary>Argument '{0}' with a value of '{1}' is not between '{2}' and '{3}!</summary>
                public const string ArgumentNotInRange =
                    "Argument '{0}' with a value of '{1}' is not between '{2}' and '{3}!";

                /// <summary>&lt;no parameter name&gt;</summary>
                public const string NoParamName = "<no parameter name>";

                /// <summary>Parameter {0} is not of Type {1}</summary>
                public const string WrongType = "Parameter {0} is not of Type {1}";
            }
        }

        #endregion Constants

        #region AssertNotNull

        /// <summary>
        /// Asserts that the argument is not <i>null</i>
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
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
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
                throw new ArgumentException(Constants.Messages.ArgumentEmpty, paramName);
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
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> contains no elements</exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName)
        {
            AssertNotEmpty(param, paramName, Constants.Messages.ArgumentNull, Constants.Messages.ArgumentEmpty);
        }

        /// <summary>
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="param"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="param"/> contains no elements</exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName, string message)
        {
            AssertNotEmpty(param, paramName, message, message);
        }

        /// <summary>
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="param">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
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
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> is <see cref="Guid.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Guid arg, string paramName)
        {
            AssertNotEmpty(arg, paramName, Constants.Messages.ArgumentEmpty);
        }

        /// <summary>
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> is <see cref="Guid.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Guid arg, string paramName, string message)
        {
            if (arg == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                                                      TypeHelper.ToNullSafeString(message)).WithAdditionalInfos(
                                                          new Dictionary<object, object> { { "arg", arg } });
            }
        }

        /// <summary>
        /// Asserts that the argument is not <i>null</i> or empty
        /// </summary>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <param name="format">The format string for the error message.</param>
        /// <param name="args">The parameters for the error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> is <see cref="Guid.Empty"/></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Guid arg, string paramName, string format, params object[] args)
        {
            AssertNotEmpty(arg, paramName, TypeHelper.SafeFormat(format, args));
        }

        #endregion AssertNotEmpty (Guid)

        #region AssertCondition

        /// <summary>
        /// Asserts that a condition is met.
        /// </summary>
        /// <param name="condition">The condition to be met</param>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <param name="format">The format string for the error message.</param>
        /// <param name="args">The parameters for the error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> does not satisfy the <paramref name="condition"/></exception>
        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object arg, string paramName, string format,
                                           params object[] args)
        {
            AssertCondition(condition, arg, paramName, TypeHelper.SafeFormat(format, args));
        }

        /// <summary>
        /// Asserts that a condition is met.
        /// </summary>
        /// <param name="condition">The condition to be met</param>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <param name="message">The error message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> does not satisfy the <paramref name="condition"/></exception>
        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object arg, string paramName, string message)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                                                      TypeHelper.ToNullSafeString(message)).WithAdditionalInfos(
                                                          new Dictionary<object, object> { { "arg", arg } });
            }
        }

        /// <summary>
        /// Asserts that a condition is met.
        /// </summary>
        /// <param name="condition">The condition to be met</param>
        /// <param name="arg">The argument to validate.</param>
        /// <param name="paramName">The name of the argument (parameter in a method signature).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> does not satisfy the <paramref name="condition"/></exception>
        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object arg, string paramName)
        {
            AssertCondition(condition, arg, paramName, Constants.Messages.ConditionNotMet);
        }

        #endregion AssertCondition

        #region InvalidSwitchValue

        /// <summary>
        /// Can be used to throw an exception if a value in a switch-case-statement is not valid.
        /// </summary>
        /// <param name="paramName">The name of the argument for the switch-case-statement.</param>
        /// <param name="arg">The actual value of the argument.</param>
        /// <exception cref="InvalidOperationException">Thrown when a value in a switch-case-statement
        /// is not valid</exception>
        [DebuggerStepThrough]
        public static void InvalidSwitchValue(object arg, string paramName)
        {
            throw new InvalidOperationException(
                TypeHelper.SafeFormat(Constants.Messages.InvalidSwitchValue,
                                      TypeHelper.ToNullSafeString(arg),
                                      TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName)));
        }

        #endregion InvalidSwitchValue

        #region AssertIsInRange

        /// <summary>
        /// Asserts that a parameter lies within a defined range
        /// </summary>
        /// <typeparam name="T">The object must be an <see cref="IComparable"/></typeparam>
        /// <param name="arg">The parameter to check</param>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="min">arg has to be greater than or equal to min</param>
        /// <param name="max">arg has to be less than or equal to max</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters
        /// <paramref name="arg"/>, <paramref name="min"/> or <paramref name="max"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> is not between
        /// <paramref name="min"/> and <paramref name="max"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T arg, string paramName, T min, T max)
            where T : IComparable
        {
            AssertIsInRange(arg, paramName, min, max, Constants.Messages.ArgumentNotInRange, paramName, arg, min, max);
        }

        /// <summary>
        /// Asserts that a parameter lies within a defined range
        /// </summary>
        /// <typeparam name="T">The object must be an <see cref="IComparable"/></typeparam>
        /// <param name="arg">The parameter to check</param>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="min">arg has to be greater than or equal to min</param>
        /// <param name="max">arg has to be less than or equal to max</param>
        /// <param name="message">The custom error message</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters
        /// <paramref name="arg"/>, <paramref name="min"/> or <paramref name="max"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> is not between
        /// <paramref name="min"/> and <paramref name="max"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T arg, string paramName, T min, T max, string message)
            where T : IComparable
        {
            AssertNotNull(arg, "arg");
            AssertNotNull(min, "min");
            AssertNotNull(max, "max");

            if (!TypeHelper.IsInRange(arg, min, max))
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                                                      TypeHelper.ToNullSafeString(message)).WithAdditionalInfos(
                                                          new Dictionary<object, object> { { "arg", arg } });
            }
        }

        /// <summary>
        /// Asserts that a parameter lies within a defined range
        /// </summary>
        /// <typeparam name="T">The object must be an <see cref="IComparable"/></typeparam>
        /// <param name="arg">The parameter to check</param>
        /// <param name="paramName">The name of the parameter</param>
        /// <param name="min">arg has to be greater than or equal to min</param>
        /// <param name="max">arg has to be less than or equal to max</param>
        /// <param name="format">The format string for the custom error message</param>
        /// <param name="args">The arguments for the custom error message</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the parameters
        /// <paramref name="arg"/>, <paramref name="min"/> or <paramref name="max"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arg"/> is not between
        /// <paramref name="min"/> and <paramref name="max"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T arg, string paramName, T min, T max, string format, params object[] args)
            where T : IComparable
        {
            AssertIsInRange(arg, paramName, min, max, TypeHelper.SafeFormat(format, args));
        }

        #endregion AssertIsInRange

        #region AssertIsType

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TTarget">The <see cref="Type"/> that must be matched by <paramref name="value"/></typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <typeparamref name="TTarget"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType<TTarget>(object value, string paramName)
        {
            AssertIsType<TTarget>(value, paramName, Constants.Messages.WrongType, paramName, typeof(TTarget).FullName);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TTarget">The <see cref="Type"/> that must be matched by <paramref name="value"/></typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The error message</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <typeparamref name="TTarget"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType<TTarget>(object value, string paramName, string message)
        {
            Type targetType = typeof(TTarget);

            AssertIsType(targetType, value, paramName, message);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <typeparamref name="TTarget"/>
        /// </summary>
        /// <typeparam name="TTarget">The <see cref="Type"/> that must be matched by <paramref name="value"/></typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="format">The format string for the error message</param>
        /// <param name="args">The arguments to put in the format string</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <typeparamref name="TTarget"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType<TTarget>(object value, string paramName, string format, params object[] args)
        {
            string message = TypeHelper.SafeFormat(format, args);

            AssertIsType<TTarget>(value, paramName, message);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <paramref name="targetType"/>
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> that must be matched by <paramref name="value"/></param>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetType"/> or <paramref name="value"/>
        /// are <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <paramref name="targetType"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType(Type targetType, object value, string paramName)
        {
            string targetTypeFullName = (targetType == null ? string.Empty : targetType.FullName);

            AssertIsType(targetType, value, paramName, Constants.Messages.WrongType, paramName, targetTypeFullName);
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <paramref name="targetType"/>
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> that must be matched by <paramref name="value"/></param>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="format">The format string for the error message</param>
        /// <param name="args">The arguments to put in the format string</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetType"/> or <paramref name="value"/>
        /// are <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <paramref name="targetType"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType(Type targetType, object value, string paramName, string format,
                                        params object[] args)
        {
            AssertIsType(targetType, value, paramName, TypeHelper.SafeFormat(format, args));
        }

        /// <summary>
        /// Asserts that the <paramref name="value"/> is of the <see cref="Type"/> specified
        /// by <paramref name="targetType"/>
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> that must be matched by <paramref name="value"/></param>
        /// <param name="value">The value to check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The error message</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetType"/> or <paramref name="value"/>
        /// are <c>null</c></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is not of <see cref="Type"/>
        /// <paramref name="targetType"/></exception>
        [DebuggerStepThrough]
        public static void AssertIsType(Type targetType, object value, string paramName, string message)
        {
            AssertNotNull(targetType, "targetType");
            AssertNotNull(value, "value");

            Type sourceType = value.GetType();

            if (string.IsNullOrEmpty(message))
            {
                message = TypeHelper.SafeFormat(Constants.Messages.WrongType, paramName, targetType.FullName);
            }

            if (!targetType.IsAssignableFrom(sourceType))
            {
                throw new ArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName), message)
                    .WithAdditionalInfos(new Dictionary<object, object>
                                             {
                                                 {"targetType", targetType},
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