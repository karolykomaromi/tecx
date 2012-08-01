namespace TecX.Unity.Injection
{
    using System;

    using TecX.Common;

    public class ConnectionStringNamingConvention : NamingConvention
    {
        public override bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return string.Equals("connectionString", name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}