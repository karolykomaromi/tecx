namespace Modular.Web.Hosting
{
    public interface IVirtualPathUtility
    {
        string ToAppRelative(string virtualPath);
    }
}