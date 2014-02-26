namespace Modular.Web.Hosting
{
    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Web.Caching;
    using System.Web.Hosting;

    public class EmbeddedResourceVirtualPathProvider : VirtualPathProvider
    {
        private readonly Assembly assembly;
        private readonly IVirtualPathUtility virtualPathUtility;

        public EmbeddedResourceVirtualPathProvider(Assembly assembly, IVirtualPathUtility virtualPathUtility)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(virtualPathUtility != null);

            this.assembly = assembly;
            this.virtualPathUtility = virtualPathUtility;
        }

        public override bool FileExists(string virtualPath)
        {
            return base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (this.IsEmbeddedResource(virtualPath))
            {
                return new EmbeddedResourceVirtualFile(virtualPath, this.assembly, this.virtualPathUtility);
            }

            return this.Previous.GetFile(virtualPath);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            return base.DirectoryExists(virtualDir);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            if (this.IsEmbeddedResource(virtualDir))
            {
                return new EmbeddedResourceVirtualDirectory(virtualDir, this.assembly, this.virtualPathUtility);
            }

            return this.Previous.GetDirectory(virtualDir);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (this.IsEmbeddedResource(virtualPath))
            {
                // TODO weberse 2014-02-26 need to figure out how to create a meaningful cache dependency
                return null;
            }

            return this.Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public bool IsEmbeddedResource(string virtualPath)
        {
            return false;
        }
    }
}