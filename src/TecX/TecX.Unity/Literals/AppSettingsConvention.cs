namespace TecX.Unity.Literals
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class AppSettingsConvention : IDependencyResolverConvention
    {
        public bool CanCreateResolver(IBuilderContext context, DependencyInfo dependency)
        {
            Guard.AssertNotNull(dependency, "dependency");
            Guard.AssertNotNull(dependency.DependencyType, "dependency.DependencyType");

            return dependency.DependencyType.IsPrimitive;
        }

        public IDependencyResolverPolicy CreateResolver(IBuilderContext context, DependencyInfo dependency)
        {
            Guard.AssertNotNull(dependency, "dependency");

            return new AppSettingsResolverPolicy(dependency.DependencyName, dependency.DependencyType);
        }
    }
}