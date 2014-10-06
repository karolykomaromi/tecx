namespace Infrastructure.Logging
{
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.Practices.Prism.Logging;

    public class DebugLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            string msg = string.Format(
                CultureInfo.CurrentCulture,
                "{0:u} {1} {2} - {3}", 
                TimeProvider.UtcNow,
                category.ToString().ToUpperInvariant(), 
                priority, 
                message);

            Debug.WriteLine(msg);
        }
    }
}
