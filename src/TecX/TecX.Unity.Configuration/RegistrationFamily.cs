using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Common;

namespace TecX.Unity.Configuration
{
    public class RegistrationFamily : IContainerConfigurator
    {
        #region Fields

        private readonly Type _from;
        private readonly Cache<string, Registration> _registrations;
        private static readonly string DefaultRegistrationKey = string.Empty;

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

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            foreach (Registration registration in _registrations)
            {
                registration.Configure(container);
            }
        }
    }
}
