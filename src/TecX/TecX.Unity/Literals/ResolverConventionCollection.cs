namespace TecX.Unity.Literals
{
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class ResolverConventionCollection
    {
        private readonly List<IDependencyResolverConvention> conventions;

        public ResolverConventionCollection()
        {
            this.conventions = new List<IDependencyResolverConvention>();
        }

        public void AddConvention(IDependencyResolverConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            this.conventions.Add(convention);
        }

        public IDependencyResolverPolicy CreateResolver(IBuilderContext context, ParameterInfo parameter)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(parameter, "parameter");

            foreach (IDependencyResolverConvention convention in this.conventions)
            {
                if (convention.CanCreateResolver(context, parameter))
                {
                    return convention.CreateResolver(context, parameter);
                }
            }

            return null;
        }
    }
}