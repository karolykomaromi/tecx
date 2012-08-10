namespace TecX.Unity.ContextualBinding.Test.ParameterBinding
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.ContextualBinding.Test.TestObjects;
    using TecX.Unity.Tracking;

    public class ConnectionStringOverride : ContextualResolverOverride
    {
        private readonly Uri url;

        private readonly string paramName;

        private readonly string connectionString;

        public ConnectionStringOverride(string url, string paramName, string connectionStringOrName)
        {
            Guard.AssertNotEmpty(url, "url");
            Guard.AssertNotEmpty(paramName, "paramName");
            Guard.AssertNotEmpty(connectionStringOrName, "connectionStringOrName");

            this.url = new Uri(url);
            this.paramName = paramName;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringOrName];

            if (settings != null)
            {
                this.connectionString = settings.ConnectionString;
            }
            else
            {
                // TODO weberse 2012-04-25 does this count as validation of the connectionstring?
                this.connectionString = new SqlConnectionStringBuilder(connectionStringOrName).ConnectionString;
            }
        }

        public override bool IsMatch(IRequest request)
        {
            OperationContext operationContext = OperationContext.Current;

            if (operationContext == null ||
                operationContext.IncomingMessageHeaders == null ||
                operationContext.IncomingMessageHeaders.To == null)
            {
                return false;
            }

            return operationContext.IncomingMessageHeaders.To == this.url;
        }

        public override void SetResolverOverrides(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "builderContext");

            context.AddResolverOverrides(new[] { new ParameterOverride(this.paramName, this.connectionString), });
        }
    }
}