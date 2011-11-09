namespace TecX.Unity.Injection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using TecX.Common;

    public class ParameterMatcher
    {
        private readonly ConstructorArgumentCollection ctorArgs;

        private readonly CompositePredicate<ConstructorInfo> filters;

        public ParameterMatcher(IEnumerable<ConstructorArgument> ctorArgs)
        {
            Guard.AssertNotNull(ctorArgs, "ctorArgs"); 
            
            this.ctorArgs = new ConstructorArgumentCollection();

            this.filters = new CompositePredicate<ConstructorInfo>();
            this.filters += this.ConstructorTakesAllArguments;
            this.filters += this.NonSatisfiedPrimitiveArgs;
            this.filters += this.ArgumentTypesMatch;

            foreach (var arg in ctorArgs)
            {
                this.ctorArgs.Add(arg);
            }
        }

        public ConstructorInfo BestMatch(IEnumerable<ConstructorInfo> ctors)
        {
            Guard.AssertNotNull(ctors, "ctors");

            // sort by number of arguments the ctor takes
            ctors = ctors.OrderByDescending(ctor => ctor.GetParameters().Length);

            IEnumerable<ConstructorInfo> potentialMatches = ctors
                .Where(ctor => !this.filters.MatchesAny(ctor));

            // no match -> exceptional situation which should cause some error))
            if (potentialMatches.Count() == 0)
            {
                throw new ArgumentException("no matching ctor found");
            }

            // one perfect match
            if (potentialMatches.Count() == 1)
            {
                return potentialMatches.Single();
            }

            // several matches -> return ctor with most arguments
            return potentialMatches
                .OrderByDescending(ctor => ctor.GetParameters().Length)
                .FirstOrDefault();
        }

        /// <summary>
        /// Filters ctor that do not take all provided arguments
        /// </summary>
        /// <param name="ctor">The ctor.</param>
        /// <returns><c>true</c> when a constructor does not take one of the provided
        /// arguments; otherwise <c>false</c></returns>
        public bool ConstructorTakesAllArguments(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");

            ParameterInfo[] parameters = ctor.GetParameters();

            foreach (var argument in this.ctorArgs)
            {
                if (!parameters.Any(p => p.Name == argument.Name))
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
            var noMatch = from p in parameters
                          where !this.ctorArgs.Names.Any(key => p.Name == key)
                          select p;

            foreach (ParameterInfo parameter in noMatch)
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

        /// <summary>
        /// Filters ctor that takes argument that matches name of a provided argument but
        /// not the provided arguments Type (e.g. ctor takes 'anotherParam' of Type string but argument
        /// 'anotherParam' of type int is provided)
        /// </summary>
        /// <param name="ctor">The ctor.</param>
        /// <returns><c>true</c> if ctor takes argument with correct argument name but wrong
        /// argument Type; otherwise <c>false</c></returns>
        public bool ArgumentTypesMatch(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");

            foreach (ParameterInfo parameter in ctor.GetParameters())
            {
                ConstructorArgument argument;
                if (this.ctorArgs.TryGetArgumentByName(parameter.Name, out argument))
                {
                    if (parameter.ParameterType != argument.Value.GetType())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}