namespace TecX.ServiceModel.AutoMagic.Configuration
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public static class ExpressionExtensions
    {
        public static TypeRegistrationExpression DiscoverService(this CreateRegistrationFamilyExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            TypeRegistrationExpression x = expression.Use(expression.From);

            x.Enrich(im => im.Add(new AutoDiscoveryProxyFactory()));

            return x;
        }

        public static TypeRegistrationExpression ServiceFromConfig(this CreateRegistrationFamilyExpression expression, string endpointConfigName)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            TypeRegistrationExpression x = expression.Use(expression.From);

            x.Enrich(im => im.Add(new ConfigFileProxyFactory(endpointConfigName)));

            return x;
        }

        public static TypeRegistrationExpression Service(this CreateRegistrationFamilyExpression expression, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            TypeRegistrationExpression x = expression.Use(expression.From);

            x.Enrich(im => im.Add(new SpecifiedProxyFactory(address, binding)));

            return x;
        }
    }
}
