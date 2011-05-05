﻿using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public abstract class Registration : IContainerConfigurator
    {
        #region Fields

        private readonly string _name;
        private readonly Type _from;
        private LifetimeManager _lifetime;

        #endregion Fields

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public LifetimeManager Lifetime
        {
            get
            {
                return _lifetime;
            }
            set
            {
                Guard.AssertNotNull(value, "Lifetime");

                _lifetime = value;
            }
        }

        public Type From
        {
            get { return _from; }
        }

        #endregion Properties

        #region c'tor

        protected Registration(Type from, string name, LifetimeManager lifetime)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(lifetime, "lifetime");

            _from = from;
            _name = name;
            _lifetime = lifetime;
        }

        #endregion c'tor

        public abstract void Configure(IUnityContainer container);
    }
}