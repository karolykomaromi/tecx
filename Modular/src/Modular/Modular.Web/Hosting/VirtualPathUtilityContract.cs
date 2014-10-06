namespace Modular.Web.Hosting
{
    using System.Diagnostics.Contracts;

    internal abstract class VirtualPathUtilityContract : IVirtualPathUtility
    {
        public string ToAppRelative(string virtualPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualPath));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return " ";
        }
    }
}