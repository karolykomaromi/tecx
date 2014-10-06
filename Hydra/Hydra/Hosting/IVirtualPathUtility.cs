namespace Hydra.Hosting
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(VirtualPathUtilityContract))]
    public interface IVirtualPathUtility
    {
        string ToAppRelative(string virtualPath);
    }

    [ContractClassFor(typeof(IVirtualPathUtility))]
    internal abstract class VirtualPathUtilityContract : IVirtualPathUtility
    {
        public string ToAppRelative(string virtualPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualPath));
            Contract.Ensures(Contract.Result<string>() != null);

            return string.Empty;
        }
    }
}