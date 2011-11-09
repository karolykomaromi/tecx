namespace TecX.Unity
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class InjectionMembers : InjectionMember
    {
        private readonly List<InjectionMember> injectionMembers;

        public InjectionMembers()
        {
            this.injectionMembers = new List<InjectionMember>();
        }

        public void Add(InjectionMember injectionMember)
        {
            Guard.AssertNotNull(injectionMember, "injectionMember");

            this.injectionMembers.Add(injectionMember);
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(serviceType, "serviceType");
            Guard.AssertNotNull(implementationType, "implementationType");
            Guard.AssertNotNull(policies, "policies");

            this.injectionMembers.ForEach(injectionMember => injectionMember.AddPolicies(serviceType, implementationType, name, policies));
        }

        public InjectionMember[] ToArray()
        {
            return this.injectionMembers.ToArray();
        }
    }
}
