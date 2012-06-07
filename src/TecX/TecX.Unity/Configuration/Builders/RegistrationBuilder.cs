namespace TecX.Unity.Configuration.Builders
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public abstract class RegistrationBuilder
    {
        private Func<Registration> compilationStrategy;

        protected RegistrationBuilder()
        {
            this.SetCompilationStrategy(this.DefaultCompilationStrategy);
        }

        public static implicit operator Registration(RegistrationBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Compile();
        }

        public Registration Compile()
        {
            return this.compilationStrategy();
        }

        public void SetCompilationStrategy(Func<Registration> compilationStrategy)
        {
            Guard.AssertNotNull(compilationStrategy, "compilationStrategy");

            this.compilationStrategy = compilationStrategy;
        }

        protected abstract Registration DefaultCompilationStrategy();
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public abstract class RegistrationBuilder<TRegistrationExpression> : RegistrationBuilder
        where TRegistrationExpression : RegistrationBuilder
    {
        private readonly Type from;

        private string name;

        private LifetimeManager lifetime;

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

        public TRegistrationExpression Named(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            this.name = name;

            return this as TRegistrationExpression;
        }

        public TRegistrationExpression LifetimeIs(LifetimeManager lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            this.lifetime = lifetime;

            return this as TRegistrationExpression;
        }

        public TRegistrationExpression AsSingleton()
        {
            return this.LifetimeIs(new ContainerControlledLifetimeManager());
        }
    }
}