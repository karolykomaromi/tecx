﻿namespace TecX.Unity.TypedFactory.Configuration
{
    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public static class ExpressionExtensions
    {
        public static TypeRegistrationExpression AsFactory(this CreateRegistrationFamilyExpression expression)
        {
            Guard.AssertNotNull(expression, "expression");

            var x = expression.Use(expression.From);

            x.Enrich(im => im.Add(new TypedFactory()));

            return x;
        }

        public static TypeRegistrationExpression AsFactory(this CreateRegistrationFamilyExpression expression, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(selector, "selector");

            var x = expression.Use(expression.From);

            x.Enrich(im => im.Add(new TypedFactory(selector)));

            return x;
        }
    }
}
