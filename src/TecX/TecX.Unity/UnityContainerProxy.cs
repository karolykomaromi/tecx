namespace TecX.Unity
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Groups;

    public abstract class UnityContainerProxy : IUnityContainer
    {
        private readonly IUnityContainer container;

        protected UnityContainerProxy(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            this.container = container;
        }

        public event EventHandler<PreRegisteringEventArgs> PreRegistering = delegate { };

        public event EventHandler<PreRegisteringInstanceEventArgs> PreRegisteringInstance = delegate { };

        public IEnumerable<ContainerRegistration> Registrations
        {
            get
            {
                return this.container.Registrations;
            }
        }

        public IUnityContainer Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected IUnityContainer Container
        {
            get
            {
                return this.container;
            }
        }

        public abstract void Dispose();

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetime, params InjectionMember[] injectionMembers)
        {
            var e = new PreRegisteringEventArgs
                {
                    From = @from,
                    To = to,
                    OriginalName = name,
                    Lifetime = lifetime,
                    InjectionMembers = injectionMembers
                };

            this.PreRegistering(this, e);

            this.container.RegisterType(e.From, e.To, e.Name, e.Lifetime, e.InjectionMembers);

            return this;
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            var e = new PreRegisteringInstanceEventArgs
                {
                    To = t, 
                    OriginalName = name, 
                    Instance = instance, 
                    Lifetime = lifetime
                };

            this.PreRegisteringInstance(this, e);

            this.container.RegisterInstance(e.To, e.Name, e.Instance, e.Lifetime);

            return this;
        }

        #region NotImplemented

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            throw new NotImplementedException();
        }

        public void Teardown(object o)
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

        #endregion NotImplemented
    }
}