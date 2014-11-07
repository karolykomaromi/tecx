namespace Hydra.Unity
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public class UseOnceParameterValue : InjectionParameterValue
    {
        private readonly InjectionParameterValue value;

        private bool used;

        public UseOnceParameterValue(InjectionParameterValue value)
        {
            this.value = value;
        }

        public override string ParameterTypeName
        {
            get { return this.value.ParameterTypeName; }
        }

        public override bool MatchesType(Type t)
        {
            return this.value.MatchesType(t);
        }

        public override IDependencyResolverPolicy GetResolverPolicy(Type typeToBuild)
        {
            if (!this.used)
            {
                try
                {
                    return this.value.GetResolverPolicy(typeToBuild);
                }
                finally
                {
                    this.used = true;
                }
            }

            return null;
        }
    }
}