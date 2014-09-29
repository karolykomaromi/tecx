namespace Hydra.Conventions
{
    using System;
    using System.Diagnostics.Contracts;
    using FubuCore;
    using Microsoft.Practices.Unity;

    public class FubuUnityServiceLocator : IServiceLocator
    {
        private readonly IUnityContainer container;

        public FubuUnityServiceLocator(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public T GetInstance<T>()
        {
            return this.container.Resolve<T>();
        }

        public object GetInstance(Type type)
        {
            return this.container.Resolve(type);
        }

        public T GetInstance<T>(string name)
        {
            return this.container.Resolve<T>(name);
        }
    }
}