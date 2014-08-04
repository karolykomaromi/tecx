namespace TecX.Unity.ContextualBinding.Test.ParameterBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class DestinationDependentConnection : InjectionMember
    {
        private readonly string url;

        private readonly string paramName;

        private readonly string connectionStringOrName;

        public DestinationDependentConnection(string url, string connectionStringOrName)
            : this(url, "connectionString", connectionStringOrName)
        {
        }

        public DestinationDependentConnection(string url, string paramName, string connectionStringOrName)
        {
            Guard.AssertNotEmpty(url, "url");
            Guard.AssertNotEmpty(paramName, "paramName");
            Guard.AssertNotEmpty(connectionStringOrName, "connectionStringOrName");

            this.url = url;
            this.paramName = paramName;
            this.connectionStringOrName = connectionStringOrName;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");

            NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);

            IContextualParameterBindingPolicy policy = policies.Get<IContextualParameterBindingPolicy>(key);

            if (policy == null)
            {
                policy = new ContextualParameterBindingPolicy();
                policies.Set<IContextualParameterBindingPolicy>(policy, key);
            }

            policy.Add(new ConnectionStringOverride(this.url, this.paramName, this.connectionStringOrName));
        }
    }
}