﻿namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;
    using TecX.Common.Extensions.Collections;

    public class ParameterMatcher
    {
        private readonly IEnumerable<ConstructorArgument> constructorArguments;

        private readonly IArgumentMatchingConventionsPolicy conventions;

        private readonly CompositePredicate<ConstructorInfo> filters;

        public ParameterMatcher(IEnumerable<ConstructorArgument> constructorArguments, IArgumentMatchingConventionsPolicy conventions)
        {
            Guard.AssertNotNull(constructorArguments, "ctorArgs");
            Guard.AssertNotNull(conventions, "conventions");

            this.constructorArguments = constructorArguments;
            this.conventions = conventions;

            this.filters = new CompositePredicate<ConstructorInfo>();
            this.filters += this.ConstructorDoesNotTakeAllArguments;
            this.filters += this.NonSatisfiedPrimitiveArgs;
        }

        public ConstructorInfo BestMatch(IEnumerable<ConstructorInfo> ctors)
        {
            Guard.AssertNotNull(ctors, "ctors");

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

        /// <summary>
        /// Filters ctor that do not take all provided arguments
        /// </summary>
        /// <param name="ctor">The ctor.</param>
        /// <returns><c>true</c> when a constructor does not take one of the provided
        /// arguments; otherwise <c>false</c></returns>
        public bool ConstructorDoesNotTakeAllArguments(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");

            ParameterInfo[] parameters = ctor.GetParameters();

            foreach (var argument in this.constructorArguments)
            {
                if (parameters.None(p => this.conventions.Matches(argument, p)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Filters ctor that takes primitive arguments (of Type string, int etc.) that
        /// are not provided
        /// </summary>
        /// <param name="ctor">The ctor.</param>
        /// <returns><c>true</c> if the ctor takes a primitive argument that is not provided;
        /// otherwise <c>false</c></returns>
        public bool NonSatisfiedPrimitiveArgs(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");

            ParameterInfo[] parameters = ctor.GetParameters();

            // find parameters not satisfied by provided args
            IEnumerable<ParameterInfo> parametersWithNoMatchingCtorArgument = parameters.Where(p => this.constructorArguments.None(a => this.conventions.Matches(a, p)));

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