namespace TecX.Unity.ContextualBinding.Configuration
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Unity.Configuration.Builders;

    public static class BuilderExtensions
    {
        public static TypeRegistrationBuilder If(this TypeRegistrationBuilder builder, Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(builder, "builder");
            Guard.AssertNotNull(predicate, "predicate");

            builder.SetCompilationStrategy(() =>
                {
                    var p = predicate;
                    var expr = builder;

                    return new ContextualTypeRegistration(expr.From, expr.To, null, expr.Lifetime, p, expr.Enrichments);
                });

            return builder;
        }

        public static InstanceRegistrationBuilder If(this InstanceRegistrationBuilder builder, Predicate<IBindingContext, IBuilderContext> predicate)
        {
            Guard.AssertNotNull(builder, "builder");
            Guard.AssertNotNull(predicate, "predicate");

            builder.SetCompilationStrategy(() =>
            {
                var p = predicate;
                var expr = builder;

                return new ContextualInstanceRegistration(expr.From, null, expr.Instance, expr.Lifetime, p);
            });

            return builder;
        }
    }
}
