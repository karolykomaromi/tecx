namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class StringAsMappingNameConvention : ParameterMatchingConvention
    {
        public override bool Matches(Parameter argument, ParameterInfo parameter)
        {
            Contract.Requires(argument != null);
            Contract.Requires(argument.Value != null);
            Contract.Requires(parameter != null);

            return this.MatchesCore(argument, parameter);
        }

        protected override bool MatchesCore(Parameter argument, ParameterInfo parameter)
        {
            if (string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase))
            {
                Type parameterType = parameter.ParameterType;

                if (argument.Value is string &&
                    parameterType != typeof(string) &&
                    (parameterType.IsClass || parameter.ParameterType.IsInterface))
                {
                    return true;
                }
            }

            return false;
        }
    }
}