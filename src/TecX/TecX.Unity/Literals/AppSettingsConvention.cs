namespace TecX.Unity.Literals
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class AppSettingsConvention : IDependencyResolverConvention
    {
        public bool CanCreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            Type t = parameter.ParameterType;

            return t == typeof(byte) || 
                t == typeof(short) || 
                t == typeof(int) || 
                t == typeof(long) ||
                t == typeof(Guid) || 
                t == typeof(string) || 
                t == typeof(bool) ||
                t == typeof(float) || 
                t == typeof(double);
        }

        public IDependencyResolverPolicy CreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            return new AppSettingsResolverPolicy(parameter.Name, parameter.ParameterType);
        }
    }
}