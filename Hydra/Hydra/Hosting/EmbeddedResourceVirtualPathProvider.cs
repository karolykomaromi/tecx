﻿namespace Hydra.Hosting
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

            this.filesByAppRelativePath = new Dictionary<string, EmbeddedFile>(visitor.Files, StringComparer.OrdinalIgnoreCase);
            this.directoriesByAppRelativePath = new Dictionary<string, EmbeddedDirectory>(visitor.Directories, StringComparer.OrdinalIgnoreCase);
        }

        public static EmbeddedResourceVirtualPathProvider Create(IVirtualPathUtility virtualPathUtility, params Assembly[] assemblies)
        {
            Contract.Requires(assemblies != null);
            Contract.Requires(assemblies.Length > 0);
            Contract.Requires(virtualPathUtility != null);
            Contract.Ensures(Contract.Result<EmbeddedResourceVirtualPathProvider>() != null);

            return new EmbeddedResourceVirtualPathProvider(virtualPathUtility, EmbeddedPathHelper.ToDirectoryStructure(assemblies));
        }

        public override bool FileExists(string virtualPath)
        {
            if (!string.IsNullOrWhiteSpace(virtualPath))
            {
                string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

                if (this.filesByAppRelativePath.ContainsKey(appRelativePath))
                {
                    return true;
                }
            }

            bool fileExists = base.FileExists(virtualPath);

            return fileExists;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (!string.IsNullOrWhiteSpace(virtualPath))
            {
                string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

                EmbeddedFile file;
                if (this.filesByAppRelativePath.TryGetValue(appRelativePath, out file))
                {
                    return file;
                }
            }

            VirtualFile virtualFile = base.GetFile(virtualPath);

            return virtualFile;
        }

        public override bool DirectoryExists(string virtualDir)
        {
            if (!string.IsNullOrWhiteSpace(virtualDir))
            {
                string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualDir) ?? string.Empty;

                if (this.directoriesByAppRelativePath.ContainsKey(appRelativePath))
                {
                    return true;
                }
            }

            bool directoryExists = base.DirectoryExists(virtualDir);

            return directoryExists;
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            if (!string.IsNullOrWhiteSpace(virtualDir))
            {
                string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualDir);

                EmbeddedDirectory dir;
                if (this.directoriesByAppRelativePath.TryGetValue(appRelativePath, out dir))
                {
                    return dir;
                }
            }

            VirtualDirectory virtualDirectory = base.GetDirectory(virtualDir);

            return virtualDirectory;
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (!string.IsNullOrWhiteSpace(virtualPath))
            {
                string appRelativePath = this.virtualPathUtility.ToAppRelative(virtualPath);

                if (this.filesByAppRelativePath.ContainsKey(appRelativePath) ||
                    this.directoriesByAppRelativePath.ContainsKey(appRelativePath))
                {
                    return null;
                }
            }

            CacheDependency cacheDependency = base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

            return cacheDependency;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.virtualPathUtility != null);
            Contract.Invariant(this.filesByAppRelativePath != null);
            Contract.Invariant(this.directoriesByAppRelativePath != null);
        }
    }
}