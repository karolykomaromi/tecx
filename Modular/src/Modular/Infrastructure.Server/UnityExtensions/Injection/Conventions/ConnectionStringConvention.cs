namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class ConnectionStringConvention : IParameterMatchingConvention
    {
        public static bool HintsAtConnectionString(string parameterName)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));

            bool hintsAtConnectionString = parameterName.IndexOf("connectionString", StringComparison.OrdinalIgnoreCase) >= 0;

            return hintsAtConnectionString;
        }

        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return HintsAtConnectionString(parameter.Name);
            }

            return false;
        }
    }
}