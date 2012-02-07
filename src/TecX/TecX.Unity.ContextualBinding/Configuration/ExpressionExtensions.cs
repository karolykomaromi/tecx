namespace TecX.Unity.ContextualBinding.Configuration
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Unity.Configuration.Expressions;

    public static class ExpressionExtensions
    {
        public static TypeRegistrationExpression If(this TypeRegistrationExpression expression, Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(predicate, "predicate");

            ((IExtensibilityInfrastructure)expression).SetCompilationStrategy(() =>
                {
                    var p = predicate;
                    var expr = expression;

                    return new ContextualTypeRegistration(expr.From, expr.To, null, expr.Lifetime, p, expr.Enrichments);
                });

            return expression;
        }

        public static InstanceRegistrationExpression If(this InstanceRegistrationExpression expression, Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(expression, "expression");
            Guard.AssertNotNull(predicate, "predicate");

            ((IExtensibilityInfrastructure)expression).SetCompilationStrategy(() =>
            {
                var p = predicate;
                var expr = expression;

                return new ContextualInstanceRegistration(expr.From, null, expr.Instance, expr.Lifetime, p);
            });

            return expression;
        }
    }
}
