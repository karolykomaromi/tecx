namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Reflection;

    public class ConnectionStringConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(Parameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.IndexOf("connectionString", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }
    }
}