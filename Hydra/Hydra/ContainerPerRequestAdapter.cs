namespace Hydra
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Allows the <see cref="UnityControllerFactory"/> to use child containers per request without pulling them from the session
    /// state itself.
    /// </summary>
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

                IUnityContainer container = HttpContext.Current.Items[Constants.ContainerKey] as IUnityContainer;

                if (container == null)
                {
                    throw new InvalidOperationException("Container not found in current Request context.");
                }

                return container;
            }
        }

        public IUnityContainer Parent
        {
            get
            {
                return this.Container.Parent;
            }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get
            {
                return this.Container.Registrations;
            }
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            throw new InvalidOperationException("Container configuration must not be altered when pulling container from request context.");
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new InvalidOperationException("Container configuration must not be altered when pulling container from request context.");
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new InvalidOperationException("Container configuration must not be altered when pulling container from request context.");
        }

        public object Configure(Type configurationInterface)
        {
            throw new InvalidOperationException("Container configuration must not be altered when pulling container from request context.");
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new InvalidOperationException("Container configuration must not be altered when pulling container from request context.");
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new InvalidOperationException("Container configuration must not be altered when pulling container from request context.");
        }

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
