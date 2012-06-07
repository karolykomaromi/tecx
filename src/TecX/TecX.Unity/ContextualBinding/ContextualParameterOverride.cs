namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ContextualParameterOverride : ContextualResolverOverride
    {
        private readonly Predicate<IBindingContext, IBuilderContext> isMatch;

        private readonly string parameterName;

        private readonly object parameterValue;

        public ContextualParameterOverride(Predicate<IBindingContext, IBuilderContext> isMatch, string parameterName, object parameterValue)
        {
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotEmpty(parameterName, "parameterName");
            Guard.AssertNotNull(parameterValue, "parameterValue");

            this.isMatch = isMatch;
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }

        public override bool IsMatch(IBindingContext bindingContext, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(bindingContext, "bindingContext");
            Guard.AssertNotNull(builderContext, "builderContext");

            return this.isMatch(bindingContext, builderContext);
        }

        public override void SetResolverOverrides(IBuilderContext context)
        {
            context.AddResolverOverrides(new[] { new ParameterOverride(this.parameterName, this.parameterValue) });
        }
    }
}