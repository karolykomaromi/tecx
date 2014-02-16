namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public class ResourceAccessor
    {
        private readonly Func<string> getResource;

        public ResourceAccessor(Func<string> getResource)
        {
            Contract.Requires(getResource != null);

            this.getResource = getResource;
        }

        public static ResourceAccessor Create(string resourceIdentifier)
        {
            Contract.Requires(!string.IsNullOrEmpty(resourceIdentifier));

            int idx = resourceIdentifier.IndexOf(".", StringComparison.Ordinal);

            string assemblyName = resourceIdentifier.Substring(0, idx) + ".Client";

            string typeName = resourceIdentifier.Substring(0, idx) + ".Assets.Resources.Labels";

            Assembly resourceAssembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.FullName.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
            
            Type resourceType = null;

            if (resourceAssembly != null)
            {
                resourceType = resourceAssembly.GetType(typeName, true);
            }

            idx = resourceIdentifier.LastIndexOf(".", StringComparison.Ordinal);

            string propertyName = resourceIdentifier.Substring(idx + 1);

            PropertyInfo property = resourceType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);

            if (property == null)
            {
                throw new PropertyNotFoundException(resourceType, propertyName);
            }

            MethodInfo getter = property.GetGetMethod(false);

            return new ResourceAccessor(() => (string)getter.Invoke(null, null));
        }

        public string GetResource()
        {
            return this.getResource();
        }
    }

    public class PropertyNotFoundException : Exception
    {
        private readonly Type type;

        private readonly string propertyName;

        public PropertyNotFoundException(Type type, string propertyName)
            : base(string.Format("No public static property named '{0}' found in class of Type '{1}'.", propertyName, type.AssemblyQualifiedName))
        {
            this.type = type;
            this.propertyName = propertyName;
        }

        public Type Type
        {
            get { return this.type; }
        }

        public string PropertyName
        {
            get { return this.propertyName; }
        }
    }
}
