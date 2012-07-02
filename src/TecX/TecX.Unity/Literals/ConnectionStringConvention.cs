namespace TecX.Unity.Literals
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ConnectionStringConvention : IDependencyResolverConvention
    {
        public bool CanCreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            return parameter.ParameterType == typeof(string) &&
                   parameter.Name.EndsWith("ConnectionString", StringComparison.OrdinalIgnoreCase);
        }

        public IDependencyResolverPolicy CreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            int index = parameter.Name.IndexOf("ConnectionString", StringComparison.OrdinalIgnoreCase);

            if (index == -1)
            {
                throw new ArgumentException("Parameter name does not contain phrase 'ConnectionString'.");
            }

            string connectionStringName = parameter.Name.Substring(0, index);

            return new ConnectionStringResolverPolicy(connectionStringName);
        }
    }
}