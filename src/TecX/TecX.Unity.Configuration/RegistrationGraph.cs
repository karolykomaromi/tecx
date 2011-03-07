using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration
{
    public class RegistrationGraph : IContainerConfigurator
    {
        private readonly RegistrationFamilyCollection _registrations;
        private readonly List<Registry> _registries;

        public List<Registry> Registries
        {
            get { return _registries; }
        }

        public RegistrationFamilyCollection RegistrationFamilies
        {
            get { return _registrations; }
        }

        public RegistrationGraph()
        {
            _registrations = new RegistrationFamilyCollection();
            _registries = new List<Registry>();
        }

        public RegistrationFamily FindFamily(Type pluginType)
        {
            return RegistrationFamilies[pluginType];
        }

        /// <summary>
        /// Add configuration to a PluginGraph with the Registry DSL
        /// </summary>
        /// <param name="action"></param>
        public void Configure(Action<Registry> action)
        {
            Guard.AssertNotNull(action, "action");

            var registry = new Registry();

            action(registry);

            registry.ConfigureRegistrationGraph(this);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            foreach(RegistrationFamily family in _registrations)
            {
                family.Configure(container);
            }
        }
    }
}
