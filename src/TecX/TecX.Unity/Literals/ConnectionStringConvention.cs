namespace TecX.Unity.Literals
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ConnectionStringConvention : IDependencyResolverConvention
    {
        public bool CanCreateResolver(IBuilderContext context, DependencyInfo dependency)
        {
            Guard.AssertNotNull(dependency, "dependency");
            Guard.AssertNotEmpty(dependency.DependencyName, "dependency.DependencyName");

            return dependency.DependencyType == typeof(string) &&
                   dependency.DependencyName.EndsWith("ConnectionString", StringComparison.OrdinalIgnoreCase);
        }

        public IDependencyResolverPolicy CreateResolver(IBuilderContext context, DependencyInfo dependency)
        {
            Guard.AssertNotNull(dependency, "dependency");
            Guard.AssertNotEmpty(dependency.DependencyName, "dependency.DependencyName");

            int index = dependency.DependencyName.IndexOf("ConnectionString", StringComparison.OrdinalIgnoreCase);

            if (index == -1)
            {
                throw new ArgumentException("Dependency name does not contain phrase 'ConnectionString'.");
            }

            string connectionStringName = dependency.DependencyName.Substring(0, index);

            return new ConnectionStringResolverPolicy(connectionStringName);
        }
    }
}