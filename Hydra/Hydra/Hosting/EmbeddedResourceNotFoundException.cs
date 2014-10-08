namespace Hydra.Hosting
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public class EmbeddedResourceNotFoundException : Exception
    {
        private readonly string resourceName;

        public EmbeddedResourceNotFoundException(Assembly assembly, string resourceName)
            : base(string.Format("Assembly '{0}' does not contain a resource named '{1}'", assembly.FullName, resourceName))
        {
            Contract.Requires(!string.IsNullOrEmpty(resourceName));
            Contract.Requires(assembly != null);

            this.resourceName = resourceName;
        }

        public string ResourceName
        {
            get { return this.resourceName; }
        }
    }
}