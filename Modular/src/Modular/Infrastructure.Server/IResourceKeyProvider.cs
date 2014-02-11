namespace Infrastructure
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ResourceKeyProviderContract))]
    public interface IResourceKeyProvider
    {
        string GetResourceKey(string listViewName, string propertyName);
    }

    [ContractClassFor(typeof(IResourceKeyProvider))]
    internal abstract class ResourceKeyProviderContract : IResourceKeyProvider
    {
        public string GetResourceKey(string listViewName, string propertyName)
        {
            Contract.Requires(!string.IsNullOrEmpty(listViewName));
            Contract.Requires(!string.IsNullOrEmpty(propertyName));

            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return "DUMMY";
        }
    }
}