namespace TecX.Unity.Factories
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class DelegateFactory : InjectionMember
    {
        public override void AddPolicies(Type serviceType, Type delegateType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(delegateType, "delegateType");
            Guard.AssertNotNull(policies, "policies");
            
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                throw new ArgumentException("'delegateType' must derive from 'System.Delegate'.", "delegateType");
            }

            DelegateFactoryBuildPlanPolicy policy = new DelegateFactoryBuildPlanPolicy(delegateType);

            policies.Set<IBuildPlanPolicy>(policy, new NamedTypeBuildKey(delegateType, name));
        }
    }
}