namespace TecX.Unity.Proxies
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class LazyProxyConfiguration
    {
        private readonly List<InjectionMember> proxyInjectionMembers;

        private readonly List<InjectionMember> serviceInjectionMembers;

        public LazyProxyConfiguration()
        {
            this.proxyInjectionMembers = new List<InjectionMember>();
            this.serviceInjectionMembers = new List<InjectionMember>();

            this.ServiceUniqueRegistrationName = Guid.NewGuid().ToString();
            this.ServiceLifetime = new TransientLifetimeManager();
            this.ProxyUniqueRegistrationName = null;
            this.ProxyLifetime = new TransientLifetimeManager();
        }

        public Type Contract { get; set; }

        public string ServiceUniqueRegistrationName { get; set; }

        public Type ServiceImplementation { get; set; }

        public LifetimeManager ServiceLifetime { get; set; }

        public InjectionMember[] ServiceInjectionMembers
        {
            get
            {
                return this.serviceInjectionMembers.ToArray();
            }
        }

        public string ProxyUniqueRegistrationName { get; set; }

        public LifetimeManager ProxyLifetime { get; set; }

        public InjectionMember[] ProxyInjectionMembers
        {
            get
            {
                List<InjectionMember> temp = new List<InjectionMember>(this.proxyInjectionMembers)
                    {
                        new InjectionConstructor(
                            new ResolvedParameter(
                            typeof(Func<>).MakeGenericType(this.Contract), this.ServiceUniqueRegistrationName))
                    };

                return temp.ToArray();
            }
        }

        public void AddProxyInjectionMembers(params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(injectionMembers, "injectionMembers");

            this.proxyInjectionMembers.AddRange(injectionMembers);
        }

        public void AddServiceInjectionMembers(params InjectionMember[] injectionMembers)
        {
            Guard.AssertNotNull(injectionMembers, "injectionMembers");

            this.serviceInjectionMembers.AddRange(injectionMembers);
        }

        public void Validate()
        {
            if (this.Contract == null)
            {
                throw new ArgumentException("LazyProxyConfiguration.Contract must not be null.");
            }

            if (!this.Contract.IsInterface)
            {
                throw new ArgumentException("LazyProxyConfiguration.Contract must be an interface.");
            }

            if (string.IsNullOrEmpty(this.ServiceUniqueRegistrationName))
            {
                throw new ArgumentException("LazyProxyConfiguration.ServiceUniqueRegistrationName must neither be empty nor null.");
            }

            if (this.ServiceImplementation == null)
            {
                throw new ArgumentException("LazyProxyConfiguration.ServiceImplementation must not be null.");
            }

            if (this.ServiceImplementation.IsAbstract || this.ServiceImplementation.IsInterface)
            {
                throw new ArgumentException("LazyProxyConfiguration.ServiceImplementation must be a concrete Type (no interface or abstract class).");
            }
        }
    }
}