namespace TecX.Unity.Injection
{
    using System;

    public class ConnectionStringNamingConvention : NamingConvention
    {
        protected override bool NameMatchesCore(string name)
        {
            return string.Equals("connectionString", name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}