namespace Infrastructure.Reflection
{
    using System;
    using System.Diagnostics.Contracts;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class DefaultNamespaceAttribute : Attribute
    {
        private readonly string defaultNamespace;

        public DefaultNamespaceAttribute(string defaultNamespace)
        {
            Contract.Requires(!string.IsNullOrEmpty(defaultNamespace));

            this.defaultNamespace = defaultNamespace;
        }

        public string DefaultNamespace
        {
            get { return this.defaultNamespace; }
        }
    }
}
