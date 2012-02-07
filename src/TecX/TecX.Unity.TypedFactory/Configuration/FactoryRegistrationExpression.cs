namespace TecX.Unity.TypedFactory.Configuration
{
    using System;

    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public class FactoryRegistrationExpression : RegistrationExpression<FactoryRegistrationExpression>
    {
        private readonly Type factoryType;

        private ITypedFactoryComponentSelector selector;

        public FactoryRegistrationExpression(CreateRegistrationFamilyExpression expression, Type factoryType)
        {
            Guard.AssertNotNull(factoryType, "factoryType");

            this.factoryType = factoryType;

            this.selector = new DefaultTypedFactoryComponentSelector();

            ((IExtensibilityInfrastructure)this).AddAlternation(expression, family => family.AddRegistration(this));
        }

        public FactoryRegistrationExpression WithSelector(ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(selector, "selector");

            this.selector = selector;

            return this;
        }

        protected override Unity.Configuration.Registration DefaultCompilationStrategy()
        {
            return new FactoryRegistration(this.factoryType, null, this.Lifetime, this.selector);
        }
    }
}