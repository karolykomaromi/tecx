using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class LifetimeMapping
    {
        private readonly Func<IRequest, bool> _matches;
        private readonly LifetimeManager _lifetimeManager;

        public LifetimeManager Lifetime
        {
            get { return _lifetimeManager; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LifetimeMapping"/> class
        /// </summary>
        public LifetimeMapping(Func<IRequest, bool> matches, LifetimeManager lifetimeManager)
        {
            Guard.AssertNotNull(matches, "matches");
            Guard.AssertNotNull(lifetimeManager, "lifetimeManager");

            _matches = matches;
            _lifetimeManager = lifetimeManager;
        }

        public bool Matches(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            return _matches(request);
        }
    }
}