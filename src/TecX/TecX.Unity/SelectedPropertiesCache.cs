namespace TecX.Unity
{
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class SelectedPropertiesCache : IPropertySelectorPolicy
    {
        private readonly IEnumerable<SelectedProperty> selectedProperties;

        public SelectedPropertiesCache(IEnumerable<SelectedProperty> selectedProperties)
        {
            Guard.AssertNotNull(selectedProperties, "selectedProperties");
            this.selectedProperties = selectedProperties;
        }

        public IEnumerable<SelectedProperty> SelectProperties(IBuilderContext context, IPolicyList resolverPolicyDestination)
        {
            return this.selectedProperties;
        }
    }
}