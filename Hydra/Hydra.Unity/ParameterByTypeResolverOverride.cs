namespace Hydra.Unity
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class ParameterByTypeResolverOverride : ResolverOverride
    {
        private readonly InjectionParameterValue value;

        public ParameterByTypeResolverOverride(object value)
        {
            this.value = new UseOnceParameterValue(InjectionParameterValue.ToParameter(value));
        }

        public override IDependencyResolverPolicy GetResolver(IBuilderContext context, Type dependencyType)
        {
            if (this.value.MatchesType(dependencyType))
            {
                return this.value.GetResolverPolicy(dependencyType);
            }

            return null;
        }
    }
}