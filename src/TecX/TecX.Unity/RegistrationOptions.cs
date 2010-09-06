using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity
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
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");
            if (lifetimeManager == null) throw new ArgumentNullException("lifetimeManager");

            From = from;
            To = to;
            Name = name;
            LifetimeManager = lifetimeManager;
            InjectionMembers = injectionMembers;
        }
    }
}
