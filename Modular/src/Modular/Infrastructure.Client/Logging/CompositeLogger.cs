namespace Infrastructure.Logging
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Prism.Logging;

    [DebuggerDisplay("Count={loggers.Count}")]
    public class CompositeLogger : ILoggerFacade
    {
        private readonly HashSet<ILoggerFacade> loggers;

        public CompositeLogger(params ILoggerFacade[] loggers)
        {
            this.loggers = new HashSet<ILoggerFacade>(loggers ?? new ILoggerFacade[0]);
        }

        public void Log(string message, Category category, Priority priority)
        {
            Contract.Requires(!string.IsNullOrEmpty(message));

            foreach (ILoggerFacade logger in this.loggers)
            {
                logger.Log(message, category, priority);
            }
        }

        public void Add(ILoggerFacade logger)
        {
            Contract.Requires(logger != null);

            this.loggers.Add(logger);
        }
    }
}
