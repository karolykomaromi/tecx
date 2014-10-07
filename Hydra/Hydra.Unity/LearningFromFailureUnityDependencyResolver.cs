namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;

    public class LearningFromFailureUnityDependencyResolver : IDependencyResolver
    {
        private readonly IDictionary<Type, Type> failedResolves;
        private readonly IDictionary<Type, IEnumerable<Type>> failedResolveAlls;
        private readonly IDependencyResolver fallback;
        private readonly IUnityContainer container;

        public LearningFromFailureUnityDependencyResolver(IDependencyResolver fallback, IUnityContainer container)
        {
            Contract.Requires(fallback != null);
            Contract.Requires(container != null);

            this.failedResolves = new Dictionary<Type, Type>();
            this.failedResolveAlls = new Dictionary<Type, IEnumerable<Type>>();
            this.fallback = fallback;
            this.container = container;
        }

        public IEnumerable<Tuple<Type, Type>> MissingMappings
        {
            get
            {
                IEnumerable<Tuple<Type, Type>> fra = from kvp in this.failedResolveAlls 
                                                     from t in kvp.Value 
                                                     select new Tuple<Type, Type>(kvp.Key, t);

                var missingMappings = this.failedResolves.Select(kvp => new Tuple<Type, Type>(kvp.Key, kvp.Value)).Union(fra).ToArray();

                return missingMappings;
            }
        }

        public object GetService(Type serviceType)
        {
            try
            {
                object service;

                Type implementationType;
                if (this.failedResolves.TryGetValue(serviceType, out implementationType))
                {
                    service = this.container.Resolve(implementationType);
                }
                else
                {
                    service = this.container.Resolve(serviceType);
                }

                return service;
            }
            catch (ResolutionFailedException ex)
            {
                object service = this.fallback.GetService(serviceType);

                Type implementationType = service.GetType();

                this.failedResolves[serviceType] = implementationType;

                return service;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                IEnumerable<object> services;

                IEnumerable<Type> implementationTypes;
                if (this.failedResolveAlls.TryGetValue(serviceType, out implementationTypes))
                {
                    // TODO weberse 2014-10-07 does not respect service lifetimes (singleton etc.)
                    List<object> s = implementationTypes.Select(implementationType => this.container.Resolve(implementationType)).ToList();

                    services = s;
                }
                else
                {
                    services = this.container.ResolveAll(serviceType);
                }

                return services;
            }
            catch (ResolutionFailedException ex)
            {
                IEnumerable<object> services = this.fallback.GetServices(serviceType);

                IEnumerable<Type> implementationTypes = services.Select(s => s.GetType());

                IEnumerable<Type> st;
                if (this.failedResolveAlls.TryGetValue(serviceType, out st))
                {
                    implementationTypes = st.Union(implementationTypes).ToList();
                }

                this.failedResolveAlls[serviceType] = implementationTypes.ToArray();

                return services;
            }
        }
    }
}