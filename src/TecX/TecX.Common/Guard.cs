namespace TecX.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    using TecX.Common.Error;
    using TecX.Common.Extensions.Error;

    /// <summary>
    /// Helper class for common method paramter validation tasks.
    /// </summary>
    public static class Guard
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
        private static class Constants
        {
            public static class Messages
            {
                /// <summary>ParameterNull '{0}' should not be NULL!</summary>
                public const string ParameterNull = "ParameterNull '{0}' must not be NULL!";

                /// <summary>paramument '{0}' should not be empty!</summary>
                public const string ParameterEmpty = "ParameterNull '{0}' must not be empty!";

                /// <summary>Condition not met!</summary>
                public const string ConditionNotMet = "Condition not met!";

                /// <summary>Invalid switch value '{0}' for parameter '{1}'.</summary>
                public const string InvalidSwitchValue = "Invalid switch value '{0}' for parameter '{1}'.";

                /// <summary>Parameter '{0}' with a value of '{1}' is not between '{2}' and '{3}!</summary>
                public const string ParameterNotInRange =
                    "Parameter '{0}' with a value of '{1}' is not between '{2}' and '{3}!";

                /// <summary>&lt;no parameter name&gt;</summary>
                public const string NoParamName = "<no parameter name>";

                /// <summary>Parameter {0} is not of Type {1}</summary>
                public const string WrongType = "Parameter {0} is not of Type {1}";
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotNull(object param, string paramName)
        {
            if (param == null)
            {
                throw new GuardArgumentNullException(paramName);
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
                throw new GuardArgumentNullException(name);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotEmpty(string param, string paramName)
        {
            if (param == null)
            {
                throw new GuardArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(param))
            {
                throw new GuardArgumentException(Constants.Messages.ParameterEmpty, paramName);
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
                throw new GuardArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(value))
            {
                string paramName = ((MemberExpression)selector.Body).Member.Name;
                throw new GuardArgumentException("String must not be empty.", paramName);
            }
        }

        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName)
        {
            if (param == null)
            {
                throw new GuardArgumentNullException(paramName, Constants.Messages.ParameterNull);
            }

            if (param.Count == 0)
            {
                throw new GuardArgumentException(paramName, Constants.Messages.ParameterEmpty).WithAdditionalInfo("param", param);
            }
        }

        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object param, string paramName, string message)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(
                    TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                    TypeHelper.ToNullSafeString(message)).WithAdditionalInfos(
                        new Dictionary<object, object> { { "param", param } });
            }
        }

        [DebuggerStepThrough]
        public static void AssertCondition(bool condition, object param, string paramName)
        {
            AssertCondition(condition, param, paramName, Constants.Messages.ConditionNotMet);
        }

        [DebuggerStepThrough]
        public static void InvalidSwitchValue(object param, string paramName)
        {
            throw new InvalidOperationException(
                TypeHelper.SafeFormat(
                    Constants.Messages.InvalidSwitchValue,
                    TypeHelper.ToNullSafeString(param),
                    TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName)));
        }

        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T param, string paramName, T min, T max) where T : IComparable
        {
            string message = TypeHelper.SafeFormat(Constants.Messages.ParameterNotInRange, paramName, param, min, max);

            AssertIsInRange(
                param, 
                paramName, 
                min, 
                max, 
                message);
        }

        [DebuggerStepThrough]
        public static void AssertIsInRange<T>(T param, string paramName, T min, T max, string message)
            where T : IComparable
        {
            AssertNotNull(param, "param");
            AssertNotNull(min, "min");
            AssertNotNull(max, "max");

            if (!TypeHelper.IsInRange(param, min, max))
            {
                throw new GuardArgumentOutOfRangeException(
                    TypeHelper.ToNullSafeString(paramName, Constants.Messages.NoParamName),
                    TypeHelper.ToNullSafeString(message)).WithAdditionalInfo("param", param);
            }
        }

        [DebuggerStepThrough]
        public static void AssertIsType(Type parameterType, object value, string paramName)
        {
            string paramTypeFullName = parameterType == null ? string.Empty : parameterType.FullName;

            string message = TypeHelper.SafeFormat(Constants.Messages.WrongType, paramName, paramTypeFullName);

            AssertIsType(parameterType, value, paramName, message);
        }

        [DebuggerStepThrough]
        public static void AssertIsType(Type parameterType, object value, string paramName, string message)
        {
            AssertNotNull(parameterType, "parameterType");
            AssertNotNull(value, "value");

            Type sourceType = value.GetType();

            if (string.IsNullOrEmpty(message))
            {
                message = TypeHelper.SafeFormat(Constants.Messages.WrongType, paramName, parameterType.FullName);
            }

            if (!parameterType.IsAssignableFrom(sourceType))
            {
                throw new GuardArgumentOutOfRangeException(TypeHelper.ToNullSafeString(paramName), message)
                    .WithAdditionalInfos(
                        new Dictionary<object, object>
                            {
                                { "parameterType", parameterType },
                                { "sourceType", sourceType },
                                { "value", value },
                                { "paramName", paramName }
                            });
            }
        }
    }
}