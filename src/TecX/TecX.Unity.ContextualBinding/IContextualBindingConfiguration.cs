using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.ContextualBinding
{
    public interface IContextualBindingConfiguration : IUnityContainerExtensionConfigurator
    {
        IContextualBindingConfiguration Register<TFrom, TTo>(Func<IRequest, bool> shouldResolve);

        IContextualBindingConfiguration Register<TFrom, TTo>(Func<IRequest, bool> shouldResolve,
                                                             LifetimeManager lifetimeManager);
    }
}