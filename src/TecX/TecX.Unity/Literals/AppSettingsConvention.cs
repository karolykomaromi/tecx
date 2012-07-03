namespace TecX.Unity.Literals
{
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class AppSettingsConvention : IDependencyResolverConvention
    {
        public bool CanCreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");
            Guard.AssertNotNull(parameter.ParameterType, "parameter.ParameterType");

            return parameter.ParameterType.IsPrimitive;
        }

        public IDependencyResolverPolicy CreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            return new AppSettingsResolverPolicy(parameter.Name, parameter.ParameterType);
        }
    }
}