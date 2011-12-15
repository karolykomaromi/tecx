namespace TecX.Common
{
    using System;
    using System.Text;

    public static class ExceptionCrawler
    {
        public static string ExtractErrorMessages(Exception exception)
        {
            Guard.AssertNotNull(exception, "exception");

            StringBuilder sb = new StringBuilder(500);

            Exception ex = exception;

            while (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);

                ex = ex.InnerException;
            }

            return sb.ToString();
        }
    }
}
