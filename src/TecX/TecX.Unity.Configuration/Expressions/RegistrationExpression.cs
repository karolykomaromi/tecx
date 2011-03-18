using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public abstract class RegistrationExpression<TRegistrationExpression> : RegistrationExpression
        where TRegistrationExpression : RegistrationExpression
    {
        private Func<LifetimeManager> _lifetimeFactory;

        public LifetimeManager Lifetime { get { return _lifetimeFactory(); } }

        protected RegistrationExpression()
        {
            _lifetimeFactory = () => new TransientLifetimeManager();
        }

        public TRegistrationExpression LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            LifetimeIs(lifetime);

            return this as TRegistrationExpression;
        }

        public TRegistrationExpression AsSingleton()
        {
            return LifetimeIs(() => new ContainerControlledLifetimeManager());
        }
    }

    public abstract class RegistrationExpression
    {
        public abstract Registration Compile();
    }
}