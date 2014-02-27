namespace Modular.Web.Hosting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Web.Caching;
    using System.Web.Hosting;

    public class EmbeddedResourceVirtualPathProvider : VirtualPathProvider
    {
        private readonly IVirtualPathUtility virtualPathUtility;
        private readonly IDictionary<string, EmbeddedFile> filesByAppRelativePath;
        private readonly IDictionary<string, EmbeddedDirectory> directoriesByAppRelativePath;

        public EmbeddedResourceVirtualPathProvider(IVirtualPathUtility virtualPathUtility, EmbeddedDirectory root)
        {
            Contract.Requires(virtualPathUtility != null);
            Contract.Requires(root != null);

            this.virtualPathUtility = virtualPathUtility;

            var visitor = new CreateLookUpVisitor();

            root.Accept(visitor);

            this.filesByAppRelativePath = visitor.Files;
            this.directoriesByAppRelativePath = visitor.Directories;
        }

        public static EmbeddedResourceVirtualPathProvider Create(Assembly assembly, IVirtualPathUtility virtualPathUtility)
        {
            Contract.Requires(assembly != null);
            Contract.Requires(virtualPathUtility != null);
            Contract.Ensures(Contract.Result<EmbeddedResourceVirtualPathProvider>() != null);

            return new EmbeddedResourceVirtualPathProvider(virtualPathUtility, EmbeddedPathHelper.ToDirectoryStructure(assembly));
        }

        public override bool FileExists(string virtualPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualPath));

            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

            if (this.filesByAppRelativePath.ContainsKey(appRelativePath))
            {
                return true;
            }

            return this.Previous.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualPath));

            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

            EmbeddedFile file;
            if (this.filesByAppRelativePath.TryGetValue(appRelativePath, out file))
            {
                return file;
            }

            return this.Previous.GetFile(virtualPath);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualDir));

            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualDir);

            if (this.directoriesByAppRelativePath.ContainsKey(appRelativePath))
            {
                return true;
            }

            return this.Previous.DirectoryExists(virtualDir);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualDir));

            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualDir);

            EmbeddedDirectory dir;
            if (this.directoriesByAppRelativePath.TryGetValue(appRelativePath, out dir))
            {
                return dir;
            }

            return this.Previous.GetDirectory(virtualDir);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

            if (this.filesByAppRelativePath.ContainsKey(appRelativePath) ||
                this.directoriesByAppRelativePath.ContainsKey(appRelativePath))
            {
                return null;
            }

            return this.Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
    }
}