namespace TecX.Unity.Injection
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ConstructorParameterMatchingConventions : UnityContainerExtension
    {
        public void Add(ParameterMatchingConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            IParameterMatchingConventionsPolicy policy = this.Context.Policies.Get<IParameterMatchingConventionsPolicy>(null);

            if (policy != null)
            {
                policy.Add(convention);
            }
        }

        protected override void Initialize()
        {
            this.Context.Policies.SetDefault<IParameterMatchingConventionsPolicy>(new DefaultMatchingConventionsPolicy());
        }
    }
}
