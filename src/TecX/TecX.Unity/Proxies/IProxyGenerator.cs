namespace TecX.Unity.Proxies
{
    using System;

    using Microsoft.Practices.Unity;

    public interface IProxyGenerator : IUnityContainerExtensionConfigurator
    {
        ////Type CreateFaultTolerantProxy(Type contract);

        Type CreateLazyInstantiationProxy(Type contract);

        Type CreateProxyWithoutTargetDummy(Type contract);
    }
}