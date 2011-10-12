using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public abstract class RegistrationExpression<TRegistrationExpression> : RegistrationExpression
        where TRegistrationExpression : RegistrationExpression
    {
        private LifetimeManager _lifetime;

        public LifetimeManager Lifetime
        {
            get { return _lifetime; }
        }

        protected RegistrationExpression()
        {
            _lifetime = new TransientLifetimeManager();
        }

        public TRegistrationExpression LifetimeIs(LifetimeManager lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            _lifetime = lifetime;

            return this as TRegistrationExpression;
        }

        public TRegistrationExpression AsSingleton()
        {
            return LifetimeIs(new ContainerControlledLifetimeManager());
        }
    }

    public abstract class RegistrationExpression
    {
        public abstract Registration Compile();

        public static implicit operator Registration(RegistrationExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            return expression.Compile();
        }
    }
}