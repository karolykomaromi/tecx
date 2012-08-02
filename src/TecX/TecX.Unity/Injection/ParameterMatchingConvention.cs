namespace TecX.Unity.Injection
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public abstract class ParameterMatchingConvention
    {
        public bool Matches(ConstructorParameter argument, ParameterInfo parameter)
        {
            Guard.AssertNotNull(argument, "argument");
            Guard.AssertNotNull(argument.Value, "argument.Value");
            Guard.AssertNotNull(parameter, "parameter");

            ResolvedParameter rp = argument.Value as ResolvedParameter;

            Type argumentValueType = rp != null ? rp.ParameterType : argument.Value.GetType();

            if (argument.Value != null &&
                parameter.ParameterType.IsAssignableFrom(argumentValueType))
            {
                return this.MatchesCore(argument, parameter);
            }

            return false;
        }

        protected abstract bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter);
    }
}
