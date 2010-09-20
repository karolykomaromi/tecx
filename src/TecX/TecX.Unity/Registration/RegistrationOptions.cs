using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class RegistrationOptions
    {
        public string Name { get; private set; }

        public Type From { get; private set; }

        public Type To { get; private set; }

        public LifetimeManager LifetimeManager { get; private set; }

        public InjectionMember[] InjectionMembers { get; private set; }

        public RegistrationOptions(Type from, Type to, string name, LifetimeManager lifetimeManager, 
            params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(lifetimeManager, "lifetimeManager");

            From = from;
            To = to;
            Name = name;
            LifetimeManager = lifetimeManager;
            InjectionMembers = injectionMembers;
        }
    }
}
