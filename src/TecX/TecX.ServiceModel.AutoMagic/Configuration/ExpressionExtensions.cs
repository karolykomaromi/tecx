namespace TecX.ServiceModel.AutoMagic.Configuration
{
    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public static class ExpressionExtensions
    {
        public static TypeRegistrationExpression Discover(this CreateRegistrationFamilyExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            var x = new TypeRegistrationExpression(expression.From, expression.From).EnrichWith(im => im.Add(new AutoDiscoveryProxyFactory()));

            return x;
        }
    }
}
