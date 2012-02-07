namespace TecX.Unity.Configuration.Expressions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public abstract class RegistrationExpression
    {
        public static implicit operator Registration(RegistrationExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            return expression.Compile();
        }

        public abstract Registration Compile();

        protected void AddAlternation(CreateRegistrationFamilyExpression expression, Action<RegistrationFamily> action)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(action, "action");

            ((IExtensibleConfiguration)expression).AddAlternation(action);
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