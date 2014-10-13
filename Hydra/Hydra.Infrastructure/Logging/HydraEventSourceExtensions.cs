namespace Hydra.Infrastructure.Logging
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public static class HydraEventSourceExtensions
    {
        public static void Warning(this HydraEventSource log, Exception exception)
        {
            Contract.Requires(log != null);
            Contract.Requires(exception != null);

            log.Warning(exception.ToString());
        }

        public static void Error(this HydraEventSource log, Exception exception)
        {
            Contract.Requires(log != null);
            Contract.Requires(exception != null);

            log.Error(exception.ToString());
        }

        public static void Critical(this HydraEventSource log, Exception exception)
        {
            Contract.Requires(log != null);
            Contract.Requires(exception != null);

            log.Critical(exception.ToString());
        }

        public static void MissingMapping(this HydraEventSource log, Type from, string name)
        {
            Contract.Requires(log != null);
            Contract.Requires(from != null);

            log.MissingMapping(from.AssemblyQualifiedName, name);
        }

        public static void ResourceTypeNotFound(this HydraEventSource log, Assembly assembly, string resourceTypeName)
        {
            Contract.Requires(log != null);
            Contract.Requires(assembly != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(resourceTypeName));

            log.ResourceTypeNotFound(assembly.GetName().FullName, resourceTypeName);
        }

        public static void ResourcePropertyNotFound(this HydraEventSource log, Assembly assembly, Type resourceType, string propertyName)
        {
            Contract.Requires(log != null);
            Contract.Requires(assembly != null);
            Contract.Requires(resourceType != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));

            log.ResourcePropertyNotFound(assembly.GetName().FullName, resourceType.FullName, propertyName);
        }
    }
}
