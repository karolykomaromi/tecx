namespace TecX.Unity.Injection
{
    using System;
    using System.Reflection;

    using TecX.Common.Extensions.Primitives;

    public class ConnectionStringMatchingConvention : ArgumentMatchingConvention
    {
        protected override bool MatchesCore(ConstructorArgument argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.Contains("connectionString", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}