namespace Infrastructure.UnityExtensions
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.ObjectBuilder2;

    public class SelectedConstructorCache : IConstructorSelectorPolicy
    {
        private readonly SelectedConstructor selectedConstructor;

        public SelectedConstructorCache(SelectedConstructor selectedConstructor)
        {
            Contract.Requires(selectedConstructor != null);

            this.selectedConstructor = selectedConstructor;
        }

        public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            return this.selectedConstructor;
        }
    }
}