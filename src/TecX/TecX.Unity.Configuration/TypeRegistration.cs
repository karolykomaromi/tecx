﻿using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class TypeRegistration : Registration
    {
        #region Fields

        private readonly Type _to;
        private readonly InjectionMember[] _enrichments;

        #endregion Fields

        #region Properties

        public Type To
        {
            get { return _to; }
        }

        public InjectionMember[] Enrichments
        {
            get { return _enrichments; }
        }

        #endregion Properties

        #region c'tor

        public TypeRegistration(Type from, Type to, string name, LifetimeManager lifetime, params InjectionMember[] enrichments)
            : base(from, name, lifetime)
        {
            Guard.AssertNotNull(to, "to");

            _to = to;
            _enrichments = enrichments;
        }

        #endregion c'tor

        public override void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            container.RegisterType(From, To, Name, Lifetime, Enrichments);
        }
    }
}