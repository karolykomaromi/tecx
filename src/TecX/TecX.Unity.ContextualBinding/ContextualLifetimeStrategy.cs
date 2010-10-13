using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualLifetimeStrategy : BuilderStrategy
    {
        private readonly IRequestHistory _history;
        private readonly ExtensionContext _context;
        private readonly IDictionary<NamedTypeBuildKey, IList<LifetimeMapping>> _lifetimeMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualLifetimeStrategy"/> class
        /// </summary>
        public ContextualLifetimeStrategy(IRequestHistory history, ExtensionContext context,
                                                 IDictionary<NamedTypeBuildKey, IList<LifetimeMapping>> lifetimeMappings)
        {
            Guard.AssertNotNull(history, "history");
            Guard.AssertNotNull(lifetimeMappings, "lifetimeMappings");
            Guard.AssertNotNull(context, "context");

            _history = history;
            _context = context;
            _lifetimeMappings = lifetimeMappings;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            //check wether we need to do something about the lifetime of the object we
            //are about to build
            IList<LifetimeMapping> lifetimeMappings;
            if (_lifetimeMappings.TryGetValue(context.BuildKey, out lifetimeMappings))
            {
                ContextualLifetimeManager lifetime = new ContextualLifetimeManager(_history, lifetimeMappings);

                SetLifetimeManager(context.BuildKey.Type, context.BuildKey.Name, lifetime);
            }
        }

        //copied from UnityDefaultBehaviorExtension
        private void SetLifetimeManager(Type lifetimeType, string name, LifetimeManager lifetimeManager)
        {
            if (lifetimeType.IsGenericTypeDefinition)
            {
                LifetimeManagerFactory factory =
                    new LifetimeManagerFactory(_context, lifetimeManager.GetType());
                _context.Policies.Set<ILifetimeFactoryPolicy>(factory,
                                                              new NamedTypeBuildKey(lifetimeType, name));
            }
            else
            {
                _context.Policies.Set<ILifetimePolicy>(lifetimeManager,
                                                       new NamedTypeBuildKey(lifetimeType, name));
                if (lifetimeManager is IDisposable)
                {
                    _context.Lifetime.Add(lifetimeManager);
                }
            }
        }
    }
}