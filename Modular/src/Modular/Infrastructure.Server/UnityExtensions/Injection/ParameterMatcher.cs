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
        private readonly IEnumerable<Parameter> constructorArguments;
        private readonly IParameterMatchingPolicy conventions;
        private readonly CompositePredicate<ConstructorInfo> filters;

        public ParameterMatcher(IEnumerable<Parameter> constructorArguments, IParameterMatchingPolicy conventions)
        {
            Contract.Requires(constructorArguments != null);
            Contract.Requires(conventions != null);

            this.constructorArguments = constructorArguments;
            this.conventions = conventions;

            this.filters = new CompositePredicate<ConstructorInfo>().Add(this.ConstructorDoesNotTakeAllArguments).Add(this.NonSatisfiedPrimitiveArgs);
        }

        public ConstructorInfo BestMatch(IEnumerable<ConstructorInfo> ctors)
        {
            Contract.Requires(ctors != null);

            // sort by number of arguments the ctor takes
            ctors = ctors.OrderByDescending(ctor => ctor.GetParameters().Length);

            List<ConstructorInfo> potentialMatches = ctors
                .Where(ctor => this.filters.MatchesNone(ctor)).ToList();

            // no match -> exceptional situation which should cause some error))
            if (potentialMatches.Count == 0)
            {
                throw new ArgumentException("no matching ctor found");
            }

            // one perfect match
            if (potentialMatches.Count == 1)
            {
                return potentialMatches.Single();
            }

            // several matches -> return ctor with most arguments
            return potentialMatches
                .OrderByDescending(ctor => ctor.GetParameters().Length)
                .First();
        }

        private bool ConstructorDoesNotTakeAllArguments(ConstructorInfo ctor)
        {
            Contract.Requires(ctor != null);

            ParameterInfo[] parameters = ctor.GetParameters();

            foreach (var argument in this.constructorArguments)
            {
                if (!parameters.Any(p => this.conventions.Matches(argument, p)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool NonSatisfiedPrimitiveArgs(ConstructorInfo ctor)
        {
            Contract.Requires(ctor != null);

            ParameterInfo[] parameters = ctor.GetParameters();

            // find parameters not satisfied by provided args
            IEnumerable<ParameterInfo> parametersWithNoMatchingCtorArgument = parameters.Where(p => !this.constructorArguments.Any(a => this.conventions.Matches(a, p)));

            foreach (ParameterInfo parameter in parametersWithNoMatchingCtorArgument)
            {
                Type paramType = parameter.ParameterType;

                if (paramType.IsEnum ||
                    paramType.IsPrimitive ||
                    paramType == typeof(string))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
