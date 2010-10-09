using System;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace TecX.Unity.ContextualBinding
{
    public interface IContextualBindingConfiguration : IUnityContainerExtensionConfigurator
    {
        IContextualBindingConfiguration Register<TFrom, TTo>(Func<IRequest, bool> shouldResolve);
    }
}