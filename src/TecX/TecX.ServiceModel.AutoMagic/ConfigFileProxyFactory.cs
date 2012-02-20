namespace TecX.ServiceModel.AutoMagic
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ConfigFileProxyFactory : InjectionMember
    {
        private readonly string endpointConfigName;

        public ConfigFileProxyFactory(string endpointConfigName)
        {
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            this.endpointConfigName = endpointConfigName;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            ConfigFileBuildPlanPolicy policy = new ConfigFileBuildPlanPolicy(this.endpointConfigName);

            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }
}