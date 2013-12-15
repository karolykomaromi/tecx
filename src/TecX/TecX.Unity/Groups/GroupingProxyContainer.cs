namespace TecX.Unity.Groups
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class GroupingProxyContainer : IUnityContainer
    {
        private readonly IUnityContainer container;

        private readonly ISemanticGroupPolicy policy;

        private string groupName;

        private Type groupParentType;

        public GroupingProxyContainer(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            this.container = container;

            this.PreRegistering += this.OnPreRegistering;
            this.PreRegisteringInstance += this.OnPreRegisteringInstance;

            this.policy = new SemanticGroupPolicy();

            this.groupName = null;
            this.groupParentType = null;
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

        public void Dispose()
        {
            this.PreRegistering -= this.OnPreRegistering;
            this.PreRegisteringInstance -= this.OnPreRegisteringInstance;

            ISemanticGroupConfigurator semantic = this.container.Configure<ISemanticGroupConfigurator>();

            if (semantic == null)
            {
                var extension = new SemanticGroupExtension();

                semantic = extension;

                this.container.AddExtension(extension);
            }

            semantic.AddPolicy(this.policy, this.groupParentType, this.groupName);
        }

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
                { To = t, OriginalName = name, Instance = instance, Lifetime = lifetime };

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

        private void OnPreRegisteringInstance(object sender, PreRegisteringInstanceEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.OriginalName) && string.IsNullOrEmpty(this.groupName))
            {
                this.groupName = args.OriginalName;
            }

            if (this.groupParentType == null)
            {
                this.groupParentType = args.To;
            }

            args.Name = this.groupName;

            this.policy.ScopedMappings.Add(new ScopedMapping { From = args.To, Name = args.Name, To = args.To });
        }

        private void OnPreRegistering(object sender, PreRegisteringEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.OriginalName) && string.IsNullOrEmpty(this.groupName))
            {
                this.groupName = args.OriginalName;
            }

            if (this.groupParentType == null)
            {
                this.groupParentType = args.To;
            }

            args.Name = this.groupName;

            this.policy.ScopedMappings.Add(new ScopedMapping { From = args.From, Name = args.Name, To = args.To });
        }
    }
}