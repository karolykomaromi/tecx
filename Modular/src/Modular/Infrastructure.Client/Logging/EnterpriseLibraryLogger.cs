namespace Infrastructure.Logging
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.Prism.Logging;

    public class EnterpriseLibraryLogger : ILoggerFacade
    {
        private readonly LogWriter writer;

        public EnterpriseLibraryLogger(LogWriter writer)
        {
            Contract.Requires(writer != null);
            this.writer = writer;
        }

        public void Log(string message, Category category, Priority priority)
        {
            this.writer.Write(message, category.ToString(), (int)priority);
        }
    }
}
