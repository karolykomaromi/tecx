namespace TecX.Unity.Injection
{
    using System;
    using System.Reflection;

    using TecX.Common.Extensions.Primitives;

    public class FileNameMatchingConvention : ArgumentMatchingConvention
    {
        protected override bool MatchesCore(ConstructorArgument argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.Contains("file", StringComparison.OrdinalIgnoreCase) ||
                       parameter.Name.Contains("path", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}