namespace TecX.Unity.Configuration.Builders
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Tracking;

    public abstract class RegistrationBuilder
    {
        public static implicit operator Registration(RegistrationBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Build();
        }

        public abstract Registration Build();
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public abstract class RegistrationBuilder<TRegistrationBuilder> : RegistrationBuilder
        where TRegistrationBuilder : RegistrationBuilder
    {
        private readonly Type from;

        private string name;

        private LifetimeManager lifetime;

        private Predicate<IRequest> predicate;

        protected RegistrationBuilder(Type @from)
        {
            Guard.AssertNotNull(from, "from");

            this.from = from;

            this.lifetime = new TransientLifetimeManager();
        }

        public LifetimeManager Lifetime
        {
            get
            {
                return this.lifetime;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Type From
        {
            get { return this.@from; }
        }

        public Predicate<IRequest> Predicate
        {
            get
            {
                return this.predicate;
            }
        }

        public TRegistrationBuilder Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            this.name = name;

            return this as TRegistrationBuilder;
        }

        public TRegistrationBuilder LifetimeIs(LifetimeManager lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            this.lifetime = lifetime;

            return this as TRegistrationBuilder;
        }

        public TRegistrationBuilder AsSingleton()
        {
            return this.LifetimeIs(new ContainerControlledLifetimeManager());
        }

        public TRegistrationBuilder If(Predicate<IRequest> predicate)
        {
            Guard.AssertNotNull(predicate, "predicate");

            this.predicate = predicate;

            return this as TRegistrationBuilder;
        }

        public TRegistrationBuilder WhenInjectedInto<TParent>()
        {
            return this.WhenInjectedInto(typeof(TParent));
        }

        public TRegistrationBuilder WhenInjectedInto(Type parentType)
        {
            Guard.AssertNotNull(parentType, "parentType");

            this.predicate = request =>
            {
                IRequest parent = request.ParentRequest;
                return parent != null && parent.Service == parentType;
            };

            return this as TRegistrationBuilder;
        }

        public TRegistrationBuilder WhenAnyAncestorNamed(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            this.predicate = request =>
                {
                    IRequest parent = request.ParentRequest;
                    while (parent != null)
                    {
                        if (string.Equals(name, parent.BuilderContext.BuildKey.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        parent = parent.ParentRequest;
                    }

                    return false;
                };

            return this as TRegistrationBuilder;
        }
    }
}