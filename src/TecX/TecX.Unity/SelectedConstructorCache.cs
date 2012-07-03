namespace TecX.Unity
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class SelectedConstructorCache : IConstructorSelectorPolicy
    {
        private readonly SelectedConstructor selectedConstructor;

        public SelectedConstructorCache(SelectedConstructor selectedConstructor)
        {
            Guard.AssertNotNull(selectedConstructor, "selectedConstructor");

            this.selectedConstructor = selectedConstructor;
        }

        public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            return this.selectedConstructor;
        }
    }
}