using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.Injection
{
    public class ParameterMatcher
    {
        #region Fields

        private readonly IDictionary<string, object> _ctorArgs;
        private readonly IEnumerable<Predicate<ConstructorInfo>> _filters;

        #endregion Fields

        #region c'tor

        private ParameterMatcher()
        {
            _ctorArgs = new Dictionary<string, object>();
            _filters = new Predicate<ConstructorInfo>[]
                           {
                               FilterByCtorTakesAllArgs, 
                               FilterByNonSatisfiedPrimitiveArgs, 
                               FilterByArgTypesFit
                           };
        }

        public ParameterMatcher(IEnumerable<KeyValuePair<string, object>> ctorArgs)
            : this()
        {
            Guard.AssertNotNull(ctorArgs, "ctorArgs");

            foreach (var arg in ctorArgs)
            {
                _ctorArgs.Add(arg);
            }
        }

        #endregion c'tor

        #region Methods

        public ConstructorInfo BestMatch(IEnumerable<ConstructorInfo> ctors)
        {
            Guard.AssertNotNull(ctors, "ctors");

            //sort by number of arguments the ctor takes
            ctors = ctors.OrderByDescending(ctor => ctor.GetParameters().Length);

            IEnumerable<ConstructorInfo> potentialMatches = ctors
                .Where(ctor => !_filters.Any(filter => filter(ctor)));

            //no match -> exceptional situation which should cause some error))
            if (potentialMatches.Count() == 0)
                throw new ArgumentException("no matching ctor found");

            //one perfect match
            if (potentialMatches.Count() == 1)
                return potentialMatches.Single();

            //several matches -> return ctor with most arguments
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
        public bool FilterByCtorTakesAllArgs(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");

            ParameterInfo[] parameters = ctor.GetParameters();

            foreach (var arg in _ctorArgs)
            {
                if (!parameters.Any(p => p.Name == arg.Key))
                    return true;
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
        public bool FilterByNonSatisfiedPrimitiveArgs(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");

            ParameterInfo[] parameters = ctor.GetParameters();

            //find parameters not satisfied by provided args
            var noMatch = from p in parameters
                          where !_ctorArgs.Keys.Any(key => p.Name == key)
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
        public bool FilterByArgTypesFit(ConstructorInfo ctor)
        {
            Guard.AssertNotNull(ctor, "ctor");
            
            foreach (ParameterInfo parameter in ctor.GetParameters())
            {
                object value;
                if (_ctorArgs.TryGetValue(parameter.Name, out value))
                {
                    if (parameter.ParameterType != value.GetType())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Methods
    }
}