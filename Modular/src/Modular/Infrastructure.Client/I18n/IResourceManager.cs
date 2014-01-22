namespace Infrastructure.I18n
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ResourceManagerContract))]
    public interface IResourceManager
    {
        string this[string key] { get; }
    }

    [ContractClassFor(typeof(IResourceManager))]
    public abstract class ResourceManagerContract : IResourceManager
    {
        public string this[string key]
        {
            get
            {
                Contract.Requires(!string.IsNullOrEmpty(key));
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                return " ";
            }
        }
    }
}
