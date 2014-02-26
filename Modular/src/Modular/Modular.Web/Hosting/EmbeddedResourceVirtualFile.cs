namespace Modular.Web.Hosting
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;

    public class EmbeddedResourceVirtualFile : VirtualFile
    {
        private readonly Assembly assembly;
        private readonly IVirtualPathUtility virtualPathUtility;

        public EmbeddedResourceVirtualFile(string virtualPath, Assembly assembly, IVirtualPathUtility virtualPathUtility)
            : base(virtualPath)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(virtualPathUtility != null);

            this.assembly = assembly;
            this.virtualPathUtility = virtualPathUtility;
        }

        public override Stream Open()
        {
            string resourceName = this.GetResourceName();

            Stream stream = this.assembly.GetManifestResourceStream(resourceName);

            return stream;
        }

        private string GetResourceName()
        {
            return string.Empty;
        }
    }
}