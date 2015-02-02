namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.Unity;

    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyResolver fallback;
        private readonly IUnityContainer container;

        public UnityDependencyResolver(IDependencyResolver fallback, IUnityContainer container)
        {
            Contract.Requires(fallback != null);
            Contract.Requires(container != null);

            this.fallback = fallback;
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                object o = this.container.Resolve(serviceType);

                return o;
            }
            catch (ResolutionFailedException ex)
            {
                HydraEventSource.Log.MissingMapping(ex.TypeRequested, ex.NameRequested);
                HydraEventSource.Log.Error(ex);

                object service = this.fallback.GetService(serviceType);

                return service;
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
                HydraEventSource.Log.MissingMapping(ex.TypeRequested, ex.NameRequested);
                HydraEventSource.Log.Error(ex);

                IEnumerable<object> services = this.fallback.GetServices(serviceType);

                return services;
            }
        }
    }
}