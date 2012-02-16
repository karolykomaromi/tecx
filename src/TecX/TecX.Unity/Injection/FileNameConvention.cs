namespace TecX.Unity.Injection
{
    using System;

    public class FileNameConvention : NamingConvention
    {
        protected override bool NameMatchesCore(string name)
        {
            return string.Equals(name, "fileName", StringComparison.InvariantCultureIgnoreCase) || 
                string.Equals(name, "file", StringComparison.InvariantCultureIgnoreCase) || 
                string.Equals(name, "path", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
