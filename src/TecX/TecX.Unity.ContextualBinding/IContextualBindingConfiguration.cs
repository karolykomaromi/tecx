﻿using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace TecX.Unity.ContextualBinding
{
    using TecX.Common;

    public interface IContextualBindingConfiguration : IUnityContainerExtensionConfigurator
    {
        void RegisterType(Type from, Type to, Predicate<IBindingContext, IBuilderContext> isMatch,
            LifetimeManager lifetime, params InjectionMember[] injectionMembers);

        void RegisterInstance(Type from, object instance, Predicate<IBindingContext, IBuilderContext> matches, LifetimeManager lifetime);

        object this[string key] { get; set; }
    }
}