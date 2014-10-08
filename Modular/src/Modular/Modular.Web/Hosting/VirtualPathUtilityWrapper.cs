namespace Modular.Web.Hosting
{
    using System.Web;

    public class VirtualPathUtilityWrapper : IVirtualPathUtility
    {
        public string ToAppRelative(string virtualPath)
        {
            return VirtualPathUtility.ToAppRelative(virtualPath);
        }
    }
}