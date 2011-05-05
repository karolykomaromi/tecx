﻿using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Common;

namespace TecX.Unity.Configuration
{
    public class RegistrationFamily : IEnumerable<Registration>
    {
        #region Fields

        private static readonly string DefaultRegistrationKey = string.Empty;
        private readonly Type _from;
        private readonly Cache<string, Registration> _registrations;

        #endregion Fields

        #region Properties

        public Type From { get { return _from; } }

        #endregion Properties

        #region c'tor

        public RegistrationFamily(Type from)
        {
            Guard.AssertNotNull(from, "from");

            _from = from;
            _registrations = new Cache<string, Registration>();
        }

        #endregion c'tor

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

        #region Implementation of IEnumerable<Registration>

        public IEnumerator<Registration> GetEnumerator()
        {
            return _registrations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion Implementation of IEnumerable<Registration>
    }
}