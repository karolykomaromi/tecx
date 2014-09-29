namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;

    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyResolver forward;
        private readonly IUnityContainer container;

        public UnityDependencyResolver(IDependencyResolver forward, IUnityContainer container)
        {
            Contract.Requires(forward != null);
            Contract.Requires(container != null);

            this.forward = forward;
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                object o = this.container.Resolve(serviceType);

                return o;
            }
            catch(ResolutionFailedException ex)
            {
                return this.forward.GetService(serviceType);
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                IEnumerable<object> all = this.container.ResolveAll(serviceType);

                return all;
            }
            catch (ResolutionFailedException ex)
            {
                return this.forward.GetServices(serviceType);
            }
        }
    }
}