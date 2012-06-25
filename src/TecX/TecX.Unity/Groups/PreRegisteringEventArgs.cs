namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    public class PreRegisteringEventArgs : EventArgs
    {
        public Type From { get; set; }

        public Type To { get; set; }

        public string OriginalName { get; set; }

        public LifetimeManager Lifetime { get; set; }

        public InjectionMember[] InjectionMembers { get; set; }

        public string Name { get; set; }
    }
}