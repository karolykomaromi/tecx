namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    public abstract class ParameterMatchingConvention
    {
        public virtual bool Matches(Parameter argument, ParameterInfo parameter)
        {
            Contract.Requires(argument != null);
            Contract.Requires(argument.Value != null);
            Contract.Requires(parameter != null);

            ResolvedParameter rp = argument.Value as ResolvedParameter;

            Type argumentValueType = rp != null ? rp.ParameterType : argument.Value.GetType();

            if (argument.Value != null &&
                parameter.ParameterType.IsAssignableFrom(argumentValueType))
            {
                return this.MatchesCore(argument, parameter);
            }

            return false;
        }

        protected abstract bool MatchesCore(Parameter argument, ParameterInfo parameter);
    }
}