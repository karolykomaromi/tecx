namespace TecX.Unity
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class SelectedMethodsCache : IMethodSelectorPolicy
    {
        private readonly IEnumerable<SelectedMethod> selectedMethods;

        public SelectedMethodsCache(IEnumerable<SelectedMethod> selectedMethods)
        {
            Guard.AssertNotNull(selectedMethods, "selectedMethods");

            this.selectedMethods = selectedMethods;
        }

        public IEnumerable<SelectedMethod> SelectMethods(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            return this.selectedMethods;
        }
    }
}