namespace Infrastructure.I18n
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ResourceManagerContract))]
    public interface IResourceManager
    {
        string this[ResxKey key] { get; }
    }

    [ContractClassFor(typeof(IResourceManager))]
    internal abstract class ResourceManagerContract : IResourceManager
    {
        public string this[ResxKey key]
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                return " ";
            }
        }
    }
}
