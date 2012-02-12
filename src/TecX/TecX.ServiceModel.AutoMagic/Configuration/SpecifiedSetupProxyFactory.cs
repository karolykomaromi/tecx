namespace TecX.ServiceModel.AutoMagic.Configuration
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class SpecifiedSetupProxyFactory : InjectionMember
    {
        private readonly EndpointAddress address;

        private readonly Binding binding;

        public SpecifiedSetupProxyFactory(EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            this.address = address;
            this.binding = binding;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            SpecifiedSetupBuildPlanPolicy policy = new SpecifiedSetupBuildPlanPolicy(this.address, this.binding);

            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(serviceType, name));
        }
    }
}