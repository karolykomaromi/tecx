using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualLifetimeManager : LifetimeManager
    {
        private readonly IRequestHistory _history;
        private readonly IList<LifetimeMapping> _mappings;
        private readonly PropertyInfo _isInUse;
        private readonly TransientLifetimeManager _default;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualLifetimeManager"/> class
        /// </summary>
        public ContextualLifetimeManager(IRequestHistory history, IList<LifetimeMapping> mappings)
        {
            Guard.AssertNotNull(history, "history");
            Guard.AssertNotNull(mappings, "mappings");

            _history = history;
            _mappings = mappings;
            _default = new TransientLifetimeManager();
            _isInUse = typeof(LifetimeManager).GetProperty("InUse",
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        #region Overrides of LifetimeManager

        public override object GetValue()
        {
            foreach (LifetimeMapping mapping in _mappings)
            {
                if (mapping.Matches(_history.Current().Parent))
                {
                    return mapping.Lifetime.GetValue();
                }
            }

            return _default.GetValue();
        }

        public override void SetValue(object newValue)
        {
            foreach (LifetimeMapping mapping in _mappings)
            {
                if (mapping.Matches(_history.Current().Parent))
                {
                    //LifetimeManager.InUse is declared internal so we need to use reflection
                    //to access the property
                    if (IsLifetimeManagerInUse(mapping.Lifetime))
                    {
                        throw new InvalidOperationException("LifetimeManager already in use");
                    }

                    mapping.Lifetime.SetValue(newValue);

                    //..same goes here. have to use reflection to signal that the lifetime manager
                    //is in use
                    SetLifetimeManagerInUse(mapping.Lifetime, true);

                    return;
                }
            }

            _default.SetValue(newValue);
        }

        public override void RemoveValue()
        {
            foreach (LifetimeMapping mapping in _mappings)
            {
                if (mapping.Matches(_history.Current().Parent))
                {
                    mapping.Lifetime.RemoveValue();

                    SetLifetimeManagerInUse(mapping.Lifetime, false);

                    return;
                }
            }

            _default.RemoveValue();
        }

        #endregion



        private bool IsLifetimeManagerInUse(LifetimeManager lifetimeManager)
        {
            return (bool)_isInUse.GetValue(lifetimeManager, null);
        }

        private void SetLifetimeManagerInUse(LifetimeManager lifetimeManager, bool inUse)
        {
            _isInUse.SetValue(lifetimeManager, inUse, null);
        }
    }
}