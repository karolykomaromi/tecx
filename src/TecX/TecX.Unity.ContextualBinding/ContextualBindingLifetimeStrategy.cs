using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingLifetimeStrategy : BuilderStrategy
    {
        private readonly IRequestHistory _history;
        private readonly ExtensionContext _context;
        private readonly IDictionary<NamedTypeBuildKey, IList<LifetimeMapping>> _lifetimeMappings;
        private readonly PropertyInfo _isInUse;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingLifetimeStrategy"/> class
        /// </summary>
        public ContextualBindingLifetimeStrategy(IRequestHistory history, ExtensionContext context,
                                                 IDictionary<NamedTypeBuildKey, IList<LifetimeMapping>> lifetimeMappings)
        {
            Guard.AssertNotNull(history, "history");
            Guard.AssertNotNull(lifetimeMappings, "lifetimeMappings");
            Guard.AssertNotNull(context, "context");

            _history = history;
            _context = context;
            _lifetimeMappings = lifetimeMappings;

            _isInUse = typeof (LifetimeManager).GetProperty("InUse",
                                                            BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            //check wether we need to do something about the lifetime of the object we
            //are about to build
            IList<LifetimeMapping> lifetimeMappings;
            if (_lifetimeMappings.TryGetValue(context.BuildKey, out lifetimeMappings))
            {
                foreach (var mapping in lifetimeMappings)
                {
                    if (mapping.Matches(_history.Current().Parent))
                    {
                        //if we found a mapping that fits the current request we set that
                        //mappings lifetime manager here
                        SetLifetimeManager(context.BuildKey.Type, context.BuildKey.Name, mapping.Lifetime);
                    }
                }
            }
        }

        //copied from UnityDefaultBehaviorExtension
        private void SetLifetimeManager(Type lifetimeType, string name, LifetimeManager lifetimeManager)
        {
            //LifetimeManager.InUse is declared internal so we need to use reflection
            //to access the property
            if (IsLifetimeManagerInUse(lifetimeManager))
            {
                throw new InvalidOperationException("LifetimeManager already in use");
            }
            if (lifetimeType.IsGenericTypeDefinition)
            {
                LifetimeManagerFactory factory =
                    new LifetimeManagerFactory(_context, lifetimeManager.GetType());
                _context.Policies.Set<ILifetimeFactoryPolicy>(factory,
                                                              new NamedTypeBuildKey(lifetimeType, name));
            }
            else
            {
                //..same goes here. have to use reflection to signal that the lifetime manager
                //is in use
                SetLifetimeManagerInUse(lifetimeManager);

                _context.Policies.Set<ILifetimePolicy>(lifetimeManager,
                                                       new NamedTypeBuildKey(lifetimeType, name));
                if (lifetimeManager is IDisposable)
                {
                    _context.Lifetime.Add(lifetimeManager);
                }
            }
        }

        private bool IsLifetimeManagerInUse(LifetimeManager lifetimeManager)
        {
            return (bool) _isInUse.GetValue(lifetimeManager, null);
        }

        private void SetLifetimeManagerInUse(LifetimeManager lifetimeManager)
        {
            _isInUse.SetValue(lifetimeManager, true, null);
        }
    }
}