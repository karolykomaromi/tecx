namespace TecX.Common.Error
{
    using System;
    using System.Linq;
    using System.Text;

    internal static class StackTraceCleaner
    {
        public static string Clean(string baseStackTrace)
        {
            var stackTrace = baseStackTrace
                .Replace("\r", string.Empty)
                .Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var stackResult = new StringBuilder();
            foreach (var stackEntry in stackTrace.Where(stackEntry => !stackEntry.StartsWith("   at TecX.Common.", StringComparison.Ordinal)))
            {
                stackResult.AppendLine(stackEntry);
            }

            return stackResult.ToString();
        }
    }
}