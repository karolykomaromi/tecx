namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Reflection;

    public class StringAsMappingNameConvention : IParameterMatchingConvention
    {
        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            if (string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase))
            {
                Type parameterType = parameter.ParameterType;

                if (argument.Value is string &&
                    parameterType != typeof(string) &&
                    (parameterType.IsClass || parameter.ParameterType.IsInterface))
                {
                    return true;
                }
            }

            return false;
        }
    }
}