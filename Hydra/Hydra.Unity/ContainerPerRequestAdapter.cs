namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Hydra.Unity.Assets;
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
                    throw new InvalidOperationException(Resources.HttpContextNotFound);
                }

                IUnityContainer container = HttpContext.Current.Items[Constants.ContainerKey] as IUnityContainer;

                if (container == null)
                {
                    throw new InvalidOperationException(Resources.ContainerNotFound);
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
            throw new InvalidOperationException(Resources.ContainerConfigurationMustNotBeAltered);
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            throw new InvalidOperationException(Resources.ContainerConfigurationMustNotBeAltered);
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            throw new InvalidOperationException(Resources.ContainerConfigurationMustNotBeAltered);
        }

        public object Configure(Type configurationInterface)
        {
            throw new InvalidOperationException(Resources.ContainerConfigurationMustNotBeAltered);
        }

        public IUnityContainer RemoveAllExtensions()
        {
            throw new InvalidOperationException(Resources.ContainerConfigurationMustNotBeAltered);
        }

        public IUnityContainer CreateChildContainer()
        {
            throw new InvalidOperationException(Resources.ContainerConfigurationMustNotBeAltered);
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            // TODO weberse 2014-10-02 for as long as I'm just playing around with this returning null is OK
            try
            {
                return this.Container.Resolve(t, name, resolverOverrides);
            }
            catch
            {
                return null;
            }
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
