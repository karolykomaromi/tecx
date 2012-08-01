namespace TecX.Unity.Injection
{
    using System;

    using TecX.Common;

    public class SpecifiedNameConvention : NamingConvention
    {
        private readonly string name;

        public SpecifiedNameConvention(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            this.name = name;
        }

        public override bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return string.Equals(this.name, name, StringComparison.OrdinalIgnoreCase);
        }
    }
}