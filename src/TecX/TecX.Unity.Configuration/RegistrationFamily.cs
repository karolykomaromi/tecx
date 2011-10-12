using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Common;

namespace TecX.Unity.Configuration
{
    public class RegistrationFamily : IEnumerable<Registration>
    {
        private static readonly string DefaultRegistrationKey = string.Empty;
        private readonly Type _from;
        private readonly Cache<string, Registration> _registrations;

        public Type From { get { return _from; } }

        public RegistrationFamily(Type from)
        {
            Guard.AssertNotNull(from, "from");

            _from = from;
            _registrations = new Cache<string, Registration>();
        }

        public void AddRegistration(Registration registration)
        {
            Guard.AssertNotNull(registration, "registration");

            _registrations[registration.Name ?? DefaultRegistrationKey] = registration;
        }

        public void LifetimeIs(Func<LifetimeManager> lifetime)
        {
            Guard.AssertNotNull(lifetime, "lifetime");

            foreach (Registration registration in _registrations)
            {
                registration.Lifetime = lifetime();
            }
        }

        public IEnumerator<Registration> GetEnumerator()
        {
            return _registrations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
