namespace Infrastructure.Logging
{
    using Microsoft.Practices.Prism.Logging;

    public class NullLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
        }
    }
}
