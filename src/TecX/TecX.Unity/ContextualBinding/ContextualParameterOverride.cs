namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Tracking;

    public class ContextualParameterOverride : ContextualResolverOverride
    {
        private readonly Predicate<IRequest> isMatch;

        private readonly string parameterName;

        private readonly object parameterValue;

        public ContextualParameterOverride(Predicate<IRequest> isMatch, string parameterName, object parameterValue)
        {
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotEmpty(parameterName, "parameterName");
            Guard.AssertNotNull(parameterValue, "parameterValue");

            this.isMatch = isMatch;
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }

        public override bool IsMatch(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            return this.isMatch(request);
        }

        public override void SetResolverOverrides(IBuilderContext context)
        {
            context.AddResolverOverrides(new[] { new ParameterOverride(this.parameterName, this.parameterValue) });
        }
    }
}