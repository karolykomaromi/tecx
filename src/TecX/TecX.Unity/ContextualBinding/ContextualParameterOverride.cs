namespace TecX.Unity.ContextualBinding
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ContextualParameterOverride : ContextualResolverOverride
    {
        private readonly Predicate<IRequest, IBuilderContext> isMatch;

        private readonly string parameterName;

        private readonly object parameterValue;

        public ContextualParameterOverride(Predicate<IRequest, IBuilderContext> isMatch, string parameterName, object parameterValue)
        {
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotEmpty(parameterName, "parameterName");
            Guard.AssertNotNull(parameterValue, "parameterValue");

            this.isMatch = isMatch;
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }

        public override bool IsMatch(IRequest request, IBuilderContext builderContext)
        {
            Guard.AssertNotNull(request, "request");
            Guard.AssertNotNull(builderContext, "builderContext");

            return this.isMatch(request, builderContext);
        }

        public override void SetResolverOverrides(IBuilderContext context)
        {
            context.AddResolverOverrides(new[] { new ParameterOverride(this.parameterName, this.parameterValue) });
        }
    }
}