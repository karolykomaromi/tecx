namespace TecX.Unity.TypedFactory.Configuration
{
    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public static class CreateRegistrationFamilyExpressionExtensions
    {
        public static TypeRegistrationExpression UseFactory(this CreateRegistrationFamilyExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            var x = expression.Use(expression.From);

            x.EnrichWith(im => im.Add(new TypedFactory()));

            return x;
        }

        public static TypeRegistrationExpression UseFactory(this CreateRegistrationFamilyExpression expression, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(selector, "selector");

            var x = expression.Use(expression.From);

            x.EnrichWith(im => im.Add(new TypedFactory(selector)));

            return x;
        }
    }
}
