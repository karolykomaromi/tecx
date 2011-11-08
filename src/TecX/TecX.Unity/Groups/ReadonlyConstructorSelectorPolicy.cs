namespace TecX.Unity.Groups
{
    using Microsoft.Practices.ObjectBuilder2;

    public class ReadonlyConstructorSelectorPolicy : IConstructorSelectorPolicy
    {
        private readonly SelectedConstructor selectedConstructor;

        public ReadonlyConstructorSelectorPolicy(SelectedConstructor selectedConstructor)
        {
            this.selectedConstructor = selectedConstructor;
        }

        public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            return selectedConstructor;
        }
    }
}