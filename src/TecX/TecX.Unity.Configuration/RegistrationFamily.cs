namespace TecX.Unity.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Configuration.Utilities;

    public class RegistrationFamily : IEnumerable<Registration>, IConfigure
    {
        private readonly Type @from;
        private readonly Cache<string, Registration> registrations;

        public RegistrationFamily(Type from)
        {
            Guard.AssertNotNull(from, "from");

            this.@from = from;
            this.registrations = new Cache<string, Registration>();
        }

        public Type From
        {
            get { return this.@from; }
        }

        public void AddRegistration(Registration registration)
        {
            Guard.AssertNotNull(registration, "registration");

            this.registrations[Guid.NewGuid().ToString()] = registration;
        }

        public void LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            foreach (Registration registration in this.registrations)
            {
                registration.Lifetime = lifetime();
            }
        }

        public IEnumerator<Registration> GetEnumerator()
        {
            return this.registrations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            foreach (Registration registration in this.registrations)
            {
                registration.Configure(container);
            }
        }
    }
}
