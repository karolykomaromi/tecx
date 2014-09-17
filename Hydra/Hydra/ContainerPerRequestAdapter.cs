namespace Hydra
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.Practices.Unity;

    public class ContainerPerRequestAdapter : IUnityContainer
    {
        public IUnityContainer Container
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("HttpContext not found.");
                }

                IUnityContainer container = HttpContext.Current.Items["unity"] as IUnityContainer;

                if (container == null)
                {
                    throw new InvalidOperationException("Container not found in current Request context.");
                }

                return container;
            }
        }

        #region Not implemented

        public IUnityContainer Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
        }

        public IUnityContainer RegisterType(
            Type @from,
            Type to,
            string name,
            LifetimeManager lifetimeManager,
            params InjectionMember[] injectionMembers)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new NotImplementedException();
        }

        public object Configure(Type configurationInterface)
        {
            throw new NotImplementedException();
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new NotImplementedException();
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        #endregion Not implemented

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return this.Container.Resolve(t, name, resolverOverrides);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return this.Container.ResolveAll(t, resolverOverrides);
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return this.Container.BuildUp(t, existing, name, resolverOverrides);
        }

        public void Teardown(object o)
        {
            this.Container.Teardown(o);
        }
    }
}
