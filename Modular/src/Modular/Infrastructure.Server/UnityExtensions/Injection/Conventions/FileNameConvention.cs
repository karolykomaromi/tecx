namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Reflection;

    public class FileNameConvention : ParameterMatchingConvention
    {
        protected override bool MatchesCore(Parameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return parameter.Name.IndexOf("file", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       parameter.Name.IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            return false;
        }
    }
}