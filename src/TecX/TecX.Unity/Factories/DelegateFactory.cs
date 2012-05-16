namespace TecX.Unity.Factories
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class DelegateFactory : InjectionMember
    {
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");

            DelegateFactoryBuildPlanPolicy policy = new DelegateFactoryBuildPlanPolicy(implementationType);

            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(implementationType, name));
        }
    }
}