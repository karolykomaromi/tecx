namespace Modular.Web.Hosting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Web.Hosting;

    public class EmbeddedResourceVirtualDirectory : VirtualDirectory
    {
        private readonly Assembly assembly;
        private readonly IVirtualPathUtility virtualPathUtility;

        private readonly List<EmbeddedResourceVirtualFile> files;
        private readonly List<EmbeddedResourceVirtualDirectory> directories;

        public EmbeddedResourceVirtualDirectory(string virtualPath, Assembly assembly, IVirtualPathUtility virtualPathUtility)
            : base(virtualPath)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(virtualPathUtility != null);

            this.assembly = assembly;
            this.virtualPathUtility = virtualPathUtility;
            this.files = new List<EmbeddedResourceVirtualFile>();
            this.directories = new List<EmbeddedResourceVirtualDirectory>();

            this.Initialize();
        }

        public override IEnumerable Directories
        {
            get
            {
                return this.directories;
            }
        }

        public override IEnumerable Files
        {
            get
            {
                return this.files;
            }
        }

        public override IEnumerable Children
        {
            get
            {
                foreach (var directory in this.directories)
                {
                    yield return directory;
                }

                foreach (var file in this.files)
                {
                    yield return file;
                }
            }
        }

        private void Initialize()
        {
        }
    }
}