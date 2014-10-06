namespace TecX.Unity.Literals
{
    using System;
    using System.Configuration;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ConnectionStringResolverPolicy : IDependencyResolverPolicy
    {
        private readonly string connectionStringName;

        public ConnectionStringResolverPolicy(string connectionStringName)
        {
            Guard.AssertNotEmpty(connectionStringName, "connectionStringName");

            this.connectionStringName = connectionStringName;
        }

        public object Resolve(IBuilderContext context)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[this.connectionStringName];

            if (setting == null)
            {
                string msg = string.Format("No ConnectionString named '{0}' found in configuration file.", this.connectionStringName);

                throw new ArgumentException(msg);
            }

            return setting.ConnectionString;
        }
    }
}