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

        protected override bool NameMatchesCore(string name)
        {
            return string.Equals(this.name, name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}