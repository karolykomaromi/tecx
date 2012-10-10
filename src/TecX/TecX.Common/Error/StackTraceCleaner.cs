namespace TecX.Common.Error
{
    using System;
    using System.Linq;

    internal static class StackTraceCleaner
    {
        public static string Clean(string baseStackTrace)
        {
            var stacktrace = baseStackTrace
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => !line.StartsWith("   at " + typeof(Guard).FullName));
            
            return string.Join(Environment.NewLine, stacktrace);
        }
    }
}