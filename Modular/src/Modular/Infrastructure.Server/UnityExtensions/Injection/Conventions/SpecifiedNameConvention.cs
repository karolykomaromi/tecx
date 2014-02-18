namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Reflection;

    public class SpecifiedNameConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(Parameter argument, ParameterInfo parameter)
        {
            if (!string.IsNullOrEmpty(argument.Name))
            {
                return string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}