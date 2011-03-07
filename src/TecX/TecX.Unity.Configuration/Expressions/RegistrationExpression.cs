using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Expressions
{
    public abstract class RegistrationExpression<TRegistrationExpression> : RegistrationExpression
        where TRegistrationExpression : RegistrationExpression
    {
        public TRegistrationExpression LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            _lifetimeFactory = lifetime;

            return this as TRegistrationExpression;
        }

        public TRegistrationExpression AsSingleton()
        {
            return LifetimeIs(() => new ContainerControlledLifetimeManager());
        }
    }

    public abstract class RegistrationExpression
    {
        #region Fields

        protected Func<LifetimeManager> _lifetimeFactory;

        #endregion Fields

        #region Properties

        public LifetimeManager Lifetime { get { return _lifetimeFactory(); } }

        #endregion Properties

        #region c'tor

        protected RegistrationExpression()
        {
            _lifetimeFactory = () => new TransientLifetimeManager();
        }

        #endregion c'tor

        public abstract Registration Compile();
    }
}