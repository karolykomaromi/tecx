namespace TecX.ServiceModel.AutoMagic.Configuration
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class AutoDiscoveryProxyFactory : InjectionMember
    {
        private readonly Uri[] scopes;

        public AutoDiscoveryProxyFactory(params Uri[] scopes)
        {
            Guard.AssertNotNull(scopes, "scopes");
            this.scopes = scopes ?? new Uri[0];
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            AutoDiscoveryBuildPlanPolicy policy = new AutoDiscoveryBuildPlanPolicy(this.scopes);

            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }
}