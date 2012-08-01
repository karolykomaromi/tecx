namespace TecX.Unity.Injection
{
    using System.Collections.Generic;
    using System.Linq;

    using TecX.Common;

    public class CompositeNamingConvention : NamingConvention
    {
        private readonly List<NamingConvention> conventions;

        public CompositeNamingConvention(IEnumerable<NamingConvention> conventions)
        {
            Guard.AssertNotNull(conventions, "conventions");

            this.conventions = new List<NamingConvention>(conventions);
        }

        public override bool NameMatches(string name)
        {
            Guard.AssertNotEmpty(name, "name");

            return this.conventions.Any(convention => convention.NameMatches(name));
        }
    }
}