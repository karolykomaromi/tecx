namespace TecX.Unity.TypedFactory.Configuration
{
    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public static class CreateRegistrationFamilyExpressionExtensions
    {
        public static FactoryRegistrationExpression UseFactory(this CreateRegistrationFamilyExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            return new FactoryRegistrationExpression(expression, expression.From);
        }
    }
}
