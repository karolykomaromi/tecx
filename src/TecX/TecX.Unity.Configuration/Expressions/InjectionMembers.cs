using System;
using System.Collections.Generic;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public class InjectionMembers : InjectionMember
    {
        private readonly List<InjectionMember> _injectionMembers;

        public InjectionMembers()
        {
            _injectionMembers = new List<InjectionMember>();
        }

        public void Add(InjectionMember injectionMember)
        {
            Guard.AssertNotNull(injectionMember, "injectionMember");

            _injectionMembers.Add(injectionMember);
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(serviceType, "serviceType");
            Guard.AssertNotNull(implementationType, "implementationType");
            Guard.AssertNotNull(policies, "policies");

            _injectionMembers.ForEach(injectionMember => injectionMember.AddPolicies(serviceType, implementationType, name, policies));
        }
    }
}
