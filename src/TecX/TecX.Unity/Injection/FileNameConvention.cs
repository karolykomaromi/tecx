namespace TecX.Unity.Injection
{
    using System;

    using TecX.Common;

    public class FileNameConvention : NamingConvention
    {
        public override bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return string.Equals(name, "fileName", StringComparison.OrdinalIgnoreCase) || 
                string.Equals(name, "file", StringComparison.OrdinalIgnoreCase) || 
                string.Equals(name, "path", StringComparison.OrdinalIgnoreCase);
        }
    }
}
