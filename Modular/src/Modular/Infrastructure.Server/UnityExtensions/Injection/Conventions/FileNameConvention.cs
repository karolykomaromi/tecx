namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class FileNameConvention : IParameterMatchingConvention
    {
        public static bool HintsAtFileName(string parameterName)
        {
            Contract.Requires(!string.IsNullOrEmpty(parameterName));

            bool hintsAtFileName = parameterName.IndexOf("file", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                   parameterName.IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0;

            return hintsAtFileName;
        }

        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return HintsAtFileName(parameter.Name);
            }

            return false;
        }
    }
}