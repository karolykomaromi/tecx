namespace Infrastructure.UnityExtensions.Injection
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class MapToRegistrationNames : InjectionMember
    {
        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Contract.Requires(implementationType != null);
            Contract.Requires(policies != null);

            policies.Set<IMapToRegistrationNamePolicy>(new MapToRegistrationNamePolicy(), new NamedTypeBuildKey(implementationType, name));
        }
    }
}