namespace TecX.Unity.Injection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    public class DefaultMatchingConventionsPolicy : IParameterMatchingConventionsPolicy
    {
        private readonly HashSet<ParameterMatchingConvention> conventions;

        public DefaultMatchingConventionsPolicy()
        {
            this.conventions = new HashSet<ParameterMatchingConvention>
                {
                    new StringAsMappingNameMatchingConvention(),
                    new SpecifiedNameMatchingConvention(),
                    new ConnectionStringMatchingConvention(),
                    new FileNameMatchingConvention(),
                    new ByTypeMatchingConvention()
                };
        }

        public bool Matches(ConstructorParameter argument, ParameterInfo parameter)
        {
            Guard.AssertNotNull(argument, "argument");
            Guard.AssertNotNull(parameter, "parameter");

            return this.conventions.Any(convention => convention.Matches(argument, parameter));
        }

        public void Add(ParameterMatchingConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            this.conventions.Add(convention);
        }
    }
}