namespace TecX.Unity.ContextualBinding
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ContextualParameter : InjectionMember
    {
        private readonly Predicate<IRequest, IBuilderContext> isMatch;

        private readonly string parameterName;

        private readonly object parameterValue;

        public ContextualParameter(Predicate<IRequest, IBuilderContext> isMatch, string parameterName, object parameterValue)
        {
            Guard.AssertNotNull(isMatch, "isMatch");
            Guard.AssertNotEmpty(parameterName, "parameterName");
            Guard.AssertNotNull(parameterValue, "parameterValue");

            this.isMatch = isMatch;
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }

        public override void AddPolicies(Type serviceType, Type implementationType, string name, IPolicyList policies)
        {
            Guard.AssertNotNull(implementationType, "implementationType");

            NamedTypeBuildKey key = new NamedTypeBuildKey(implementationType, name);

            IContextualParameterBindingPolicy policy = policies.Get<IContextualParameterBindingPolicy>(key);

            if (policy == null)
            {
                policy = new ContextualParameterBindingPolicy();
                policies.Set<IContextualParameterBindingPolicy>(policy, key);
            }

            policy.Add(new ContextualParameterOverride(this.isMatch, this.parameterName, this.parameterValue));
        }
    }
}