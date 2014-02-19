namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System;
    using System.Reflection;

    public class SpecifiedNameConvention : IParameterMatchingConvention
    {
        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            // argument and param name match and the argument value is of a type
            // that is assignable to the parameter type
            if (!string.IsNullOrEmpty(argument.Name) &&
                string.Equals(argument.Name, parameter.Name, StringComparison.OrdinalIgnoreCase) &&
                argument.Value != null && 
                parameter.ParameterType.IsInstanceOfType(argument.Value))
            {
                return true;
            }

            return false;
        }
    }
}