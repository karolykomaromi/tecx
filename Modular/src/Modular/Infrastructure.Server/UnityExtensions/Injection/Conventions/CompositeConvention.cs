namespace Infrastructure.UnityExtensions.Injection.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class CompositeConvention : IParameterMatchingConvention
    {
        private readonly HashSet<IParameterMatchingConvention> conventions;

        public CompositeConvention(params IParameterMatchingConvention[] conventions)
        {
            this.conventions = new HashSet<IParameterMatchingConvention>(conventions ?? new IParameterMatchingConvention[0]);
        }

        public bool IsMatch(Parameter argument, ParameterInfo parameter)
        {
            return this.conventions.Any(convention => convention.IsMatch(argument, parameter));
        }
    }
}
