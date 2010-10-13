using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBuildKeyMapping
    {
        private readonly Func<IRequest, bool> _matches;
        private readonly Type _mapTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBuildKeyMapping"/> class
        /// </summary>
        public ContextualBuildKeyMapping(Func<IRequest, bool> matches, Type mapTo)
        {
            Guard.AssertNotNull(matches, "matches");
            Guard.AssertNotNull(mapTo, "mapTo");

            _matches = matches;
            _mapTo = mapTo;
        }

        public bool Matches(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            return _matches(request);
        }

        public NamedTypeBuildKey MapTo
        {
            get { return new NamedTypeBuildKey(_mapTo); }
        }
    }
}