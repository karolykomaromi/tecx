using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class TypeRegistration : Registration
    {
        private readonly Type _from;
        private readonly Type _to;
        private readonly LifetimeManager _lifetime;
        private readonly InjectionMember[] _enrichments;

        public Type From
        {
            get { return _from; }
        }

        public LifetimeManager Lifetime
        {
            get { return _lifetime; }
        }

        public InjectionMember[] Enrichments
        {
            get { return _enrichments; }
        }

        public Type To
        {
            get { return _to; }
        }

        public TypeRegistration(Type from, Type to, string name, LifetimeManager lifetime, params InjectionMember[] enrichments)
            : base(name)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(lifetime, "lifetime");

            _to = to;
            _lifetime = lifetime;
            _enrichments = enrichments;
            _from = from;
        }

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterType(From, To, Name, Lifetime, Enrichments);
        }
    }
}