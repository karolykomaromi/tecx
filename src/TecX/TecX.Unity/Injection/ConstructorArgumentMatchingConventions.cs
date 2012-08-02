namespace TecX.Unity.Injection
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ConstructorArgumentMatchingConventions : UnityContainerExtension
    {
        public void Add(ArgumentMatchingConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            IArgumentMatchingConventionsPolicy policy = this.Context.Policies.Get<IArgumentMatchingConventionsPolicy>(null);

            if (policy != null)
            {
                policy.Add(convention);
            }
        }

        protected override void Initialize()
        {
            this.Context.Policies.SetDefault<IArgumentMatchingConventionsPolicy>(new DefaultMatchingConventionsPolicy());
        }
    }
}
