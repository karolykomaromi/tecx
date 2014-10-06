namespace TecX.Unity.Injection
{
    using System;
    using System.Reflection;

    using TecX.Common.Extensions.Primitives;
    using TecX.Unity.Utility;

    public class ConnectionStringMatchingConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.Contains("connectionString", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}