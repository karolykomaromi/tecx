namespace TecX.Unity.Injection
{
    using System;
    using System.Reflection;

    using TecX.Common.Extensions.Primitives;
    using TecX.Unity.Utility;

    public class FileNameMatchingConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(ConstructorParameter argument, ParameterInfo parameter)
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