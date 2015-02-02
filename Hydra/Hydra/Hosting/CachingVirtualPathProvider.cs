namespace Hydra.Hosting
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.Caching;
    using System.Web.Hosting;

    public class CachingVirtualPathProvider : VirtualPathProvider, IDisposable
    {
        private readonly VirtualPathProvider inner;
        private readonly IDictionary<string, bool> fileExists;
        private readonly IDictionary<string, bool> directoryExists;
        private readonly IDictionary<string, VirtualFile> files;
        private readonly IDictionary<string, VirtualDirectory> directories;

        public CachingVirtualPathProvider(VirtualPathProvider inner)
        {
            Contract.Requires(inner != null);

            this.inner = inner;
            this.fileExists = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            this.directoryExists = new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            this.files = new ConcurrentDictionary<string, VirtualFile>(StringComparer.OrdinalIgnoreCase);
            this.directories = new ConcurrentDictionary<string, VirtualDirectory>(StringComparer.OrdinalIgnoreCase);
        }

        public override bool FileExists(string virtualPath)
        {
            bool exists;
            if (!this.fileExists.TryGetValue(virtualPath, out exists))
            {
                exists = this.inner.FileExists(virtualPath);
                this.fileExists.Add(virtualPath, exists);
            }

            return exists;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            VirtualFile file;
            if (!this.files.TryGetValue(virtualPath, out file))
            {
                file = this.inner.GetFile(virtualPath);
                this.files.Add(virtualPath, file);
            }

            return file;
        }

        public override bool DirectoryExists(string virtualDir)
        {
            bool exists;
            if (!this.directoryExists.TryGetValue(virtualDir, out exists))
            {
                exists = this.inner.DirectoryExists(virtualDir);
                this.directoryExists.Add(virtualDir, exists);
            }

            return exists;
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            VirtualDirectory directory;
            if (!this.directories.TryGetValue(virtualDir, out directory))
            {
                directory = this.inner.GetDirectory(virtualDir);
                this.directories.Add(virtualDir, directory);
            }

            return directory;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.directories.Clear();
                this.directoryExists.Clear();
                this.files.Clear();
                this.fileExists.Clear();
            }
        }
    }
}