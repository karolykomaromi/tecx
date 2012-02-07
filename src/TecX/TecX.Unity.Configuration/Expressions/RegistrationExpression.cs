namespace TecX.Unity.Configuration.Expressions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public interface IExtensibilityInfrastructure
    {
        void AddAlternation(CreateRegistrationFamilyExpression expression, Action<RegistrationFamily> action);

        void SetCompilationStrategy(Func<Registration> compilationStrategy);
    }

    public abstract class RegistrationExpression : IExtensibilityInfrastructure
    {
        private Func<Registration> compilationStrategy;

        protected RegistrationExpression()
        {
            ((IExtensibilityInfrastructure)this).SetCompilationStrategy(this.DefaultCompilationStrategy);
        }

        public static implicit operator Registration(RegistrationExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            return expression.Compile();
        }

        public Registration Compile()
        {
            return this.compilationStrategy();
        }

        protected abstract Registration DefaultCompilationStrategy();

        void IExtensibilityInfrastructure.AddAlternation(CreateRegistrationFamilyExpression expression, Action<RegistrationFamily> action)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(action, "action");

            expression.AddAlternation(action);
        }

        void IExtensibilityInfrastructure.SetCompilationStrategy(Func<Registration> compilationStrategy)
        {
            Guard.AssertNotNull(compilationStrategy, "compilationStrategy");

            this.compilationStrategy = compilationStrategy;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public abstract class RegistrationExpression<TRegistrationExpression> : RegistrationExpression
        where TRegistrationExpression : RegistrationExpression
    {
        private LifetimeManager lifetime;

        protected RegistrationExpression()
        {
            this.lifetime = new TransientLifetimeManager();
        }

        public LifetimeManager Lifetime
        {
            get
            {
                return this.lifetime;
            }
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