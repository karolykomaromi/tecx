namespace TecX.Unity.Injection
{
    using System;
    using System.Reflection;

    public class SpecifiedNameMatchingConvention : ArgumentMatchingConvention
    {
        protected override bool MatchesCore(ConstructorArgument argument, ParameterInfo parameter)
        {
            if (!string.IsNullOrEmpty(argument.Name))
            {
                return string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}