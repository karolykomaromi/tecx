namespace TecX.Unity.Factories
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using TecX.Common;

    public class OneTimeTypeMatchParameterOverride : ResolverOverride
    {
        private readonly InjectionParameterValue parameterValue;

        private bool used;

        public OneTimeTypeMatchParameterOverride(object value)
        {
            Guard.AssertNotNull(value, "value");

            this.used = false;
            this.parameterValue = InjectionParameterValue.ToParameter(value);
        }

        public override IDependencyResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
        {
            if (!this.used && this.parameterValue.MatchesType(dependencyType))
            {
                this.used = true;

                return this.parameterValue.GetResolverPolicy(dependencyType);
            }

            return null;
        }
    }
}