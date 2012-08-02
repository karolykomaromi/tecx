namespace TecX.Unity.Injection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    public class DefaultMatchingConventionsPolicy : IArgumentMatchingConventionsPolicy
    {
        private readonly HashSet<ArgumentMatchingConvention> conventions;

        public DefaultMatchingConventionsPolicy()
        {
            this.conventions = new HashSet<ArgumentMatchingConvention>
                {
                    new SpecifiedNameMatchingConvention(),
                    new ConnectionStringMatchingConvention(),
                    new FileNameMatchingConvention(),
                    new ByTypeMatchingConvention()
                };
        }

        public bool Matches(ConstructorArgument argument, ParameterInfo parameter)
        {
            Guard.AssertNotNull(argument, "argument");
            Guard.AssertNotNull(parameter, "parameter");

            return this.conventions.Any(convention => convention.Matches(argument, parameter));
        }

        public void Add(ArgumentMatchingConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            this.conventions.Add(convention);
        }
    }
}