namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public class ParameterMatchingPolicy : IParameterMatchingPolicy
    {
        private readonly HashSet<ParameterMatchingConvention> conventions;

        public ParameterMatchingPolicy()
        {
            this.conventions = new HashSet<ParameterMatchingConvention>
                {
                    new StringAsMappingNameConvention(),
                    new SpecifiedNameConvention(),
                    new ConnectionStringConvention(),
                    new FileNameConvention(),
                    new ByTypeConvention()
                };
        }

        public bool Matches(Parameter argument, ParameterInfo parameter)
        {
            Contract.Requires(argument != null);
            Contract.Requires(parameter != null);

            return this.conventions.Any(convention => convention.Matches(argument, parameter));
        }

        public void Add(ParameterMatchingConvention convention)
        {
            Contract.Requires(convention != null);

            this.conventions.Add(convention);
        }
    }
}