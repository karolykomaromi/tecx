namespace Hydra.Hosting
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

        public static EmbeddedResourceVirtualPathProvider Create(IVirtualPathUtility virtualPathUtility, params Assembly[] assemblies)
        {
            Contract.Requires(assemblies != null);
            Contract.Requires(virtualPathUtility != null);
            Contract.Ensures(Contract.Result<EmbeddedResourceVirtualPathProvider>() != null);

            return new EmbeddedResourceVirtualPathProvider(virtualPathUtility, EmbeddedPathHelper.ToDirectoryStructure(assemblies));
        }

        public override bool FileExists(string virtualPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualPath));

            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

            if (this.filesByAppRelativePath.ContainsKey(appRelativePath))
            {
                return true;
            }

            bool fileExists = base.FileExists(virtualPath);

            return fileExists;
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

            VirtualFile virtualFile = base.GetFile(virtualPath);

            return virtualFile;
        }

        public override bool DirectoryExists(string virtualDir)
        {
            Contract.Requires(!string.IsNullOrEmpty(virtualDir));

            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualDir);

            if (this.directoriesByAppRelativePath.ContainsKey(appRelativePath))
            {
                return true;
            }

            bool directoryExists = base.DirectoryExists(virtualDir);

            return directoryExists;
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

            VirtualDirectory virtualDirectory = base.GetDirectory(virtualDir);

            return virtualDirectory;
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

            if (this.filesByAppRelativePath.ContainsKey(appRelativePath) ||
                this.directoriesByAppRelativePath.ContainsKey(appRelativePath))
            {
                return null;
            }

            CacheDependency cacheDependency = base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return cacheDependency;
        }
    }
}