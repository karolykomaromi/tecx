namespace Hydra.Hosting
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;

    [DebuggerDisplay("File={AppRelativePath} Resource={ResourceName}")]
    public class EmbeddedFile : VirtualFile, IEquatable<EmbeddedFile>
    {
        private readonly string resourceName;
        private readonly Assembly assembly;
        private readonly string appRelativePath;

        public EmbeddedFile(string appRelativePath, string resourceName, Assembly assembly)
            : base(appRelativePath)
        {
            Contract.Requires(!string.IsNullOrEmpty(appRelativePath));
            Contract.Requires(!string.IsNullOrEmpty(resourceName));
            Contract.Requires(assembly != null);

            this.appRelativePath = appRelativePath;
            this.resourceName = resourceName;
            this.assembly = assembly;
        }

        public string ResourceName
        {
            get { return this.resourceName; }
        }

        public Assembly Assembly
        {
            get { return this.assembly; }
        }

        public string AppRelativePath
        {
            get { return this.appRelativePath; }
        }

        public override Stream Open()
        {
            Stream stream = this.Assembly.GetManifestResourceStream(this.resourceName);

            if (stream == null)
            {
                throw new EmbeddedResourceNotFoundException(this.Assembly, this.resourceName);
            }

            return stream;
        }

        public void Accept(EmbeddedVisitor visitor)
        {
            Contract.Requires(visitor != null);

            visitor.Visit(this);
        }

        public override int GetHashCode()
        {
            return this.ResourceName.GetHashCode();
        }

        public bool Equals(EmbeddedFile other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            bool equals = string.Equals(this.AppRelativePath, other.AppRelativePath, StringComparison.OrdinalIgnoreCase);

            return equals;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            EmbeddedFile other = obj as EmbeddedFile;

            if (other == null)
            {
                return false;
            }

            bool equals = string.Equals(this.ResourceName, other.ResourceName, StringComparison.Ordinal);

            return equals;
        }
    }
}