namespace TecX.Unity.Configuration
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public abstract class Registration : IContainerConfigurator
    {
        private readonly string name;
        private readonly Type @from;
        private LifetimeManager lifetime;

        protected Registration(Type from, string name, LifetimeManager lifetime)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(lifetime, "lifetime");

            this.@from = from;
            this.name = name;
            this.lifetime = lifetime;
        }

        public string Name
        {
            get { return this.name; }
        }

        public LifetimeManager Lifetime
        {
            get
            {
                return this.lifetime;
            }

            set
            {
                Guard.AssertNotNull(value, "Lifetime");

                this.lifetime = value;
            }
        }

        public Type From
        {
            get { return this.@from; }
        }

        public abstract void Configure(IUnityContainer container);
    }
}
