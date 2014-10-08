namespace Hydra.Infrastructure.Logging
{
    using System;
    using System.Diagnostics.Contracts;

    public static class HydraEventSourceExtensions
    {
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

        public static void MissingMapping(this HydraEventSource log, Type from, Type to)
        {
            Contract.Requires(log != null);
            Contract.Requires(from != null);
            Contract.Requires(to != null);

            log.MissingMapping(from.AssemblyQualifiedName, to.AssemblyQualifiedName);
        }
    }
}
