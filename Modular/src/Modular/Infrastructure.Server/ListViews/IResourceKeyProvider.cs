namespace Infrastructure.ListViews
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ResourceKeyProviderContract))]
    public interface IResourceKeyProvider
    {
        string GetResourceKey(ListViewId listViewId, string propertyName);
    }

    [ContractClassFor(typeof(IResourceKeyProvider))]
    internal abstract class ResourceKeyProviderContract : IResourceKeyProvider
    {
        public string GetResourceKey(ListViewId listViewId, string propertyName)
        {
            Contract.Requires(listViewId != ListViewId.Empty);
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return "DUMMY";
        }
    }
}