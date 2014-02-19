namespace Infrastructure.UnityExtensions.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Conventions;

    public class ParameterMatcher
    {
        private readonly IEnumerable<Parameter> parameters;
        private readonly IParameterMatchingConvention convention;

        public ParameterMatcher(IEnumerable<Parameter> parameters, IParameterMatchingConvention convention)
        {
            Contract.Requires(parameters != null);
            Contract.Requires(convention != null);

            this.parameters = parameters;
            this.convention = convention;
        }

        public ConstructorInfo BestMatch(IEnumerable<ConstructorInfo> ctors)
        {
            Contract.Requires(ctors != null);

            // sort by number of arguments the ctor takes
            ctors = ctors.OrderByDescending(ctor => ctor.GetParameters().Length);

            var potentialMatches = ctors.Where(ctor => this.ConstructorTakesAllParameters(ctor) && this.AllPrimitiveArgsSatisfied(ctor)).ToArray();

            // no match -> exceptional situation which should cause some error))
            if (potentialMatches.Length == 0)
            {
                throw new ArgumentException("No matching ctor found");
            }

            // one perfect match
            if (potentialMatches.Length == 1)
            {
                return potentialMatches.Single();
            }

            // several matches -> return ctor with most arguments
            return potentialMatches
                .OrderByDescending(ctor => ctor.GetParameters().Length)
                .First();
        }

        public bool ConstructorTakesAllParameters(ConstructorInfo ctor)
        {
            Contract.Requires(ctor != null);

            ParameterInfo[] parameters = ctor.GetParameters();

            foreach (var argument in this.parameters)
            {
                if (parameters.Any(parameter => this.convention.IsMatch(argument, parameter)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AllPrimitiveArgsSatisfied(ConstructorInfo ctor)
        {
            Contract.Requires(ctor != null);

            ParameterInfo[] arguments = ctor.GetParameters();

            // find parameters not satisfied by provided args
            IEnumerable<ParameterInfo> argumentWithNoMatchingParameter = arguments.Where(p => !this.parameters.Any(a => this.convention.IsMatch(a, p)));

            foreach (ParameterInfo argument in argumentWithNoMatchingParameter)
            {
                Type paramType = argument.ParameterType;

                if (paramType.IsEnum ||
                    paramType.IsPrimitive ||
                    paramType == typeof(string))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
