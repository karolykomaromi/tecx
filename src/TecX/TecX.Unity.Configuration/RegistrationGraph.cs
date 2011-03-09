using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Microsoft.Practices.Unity;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration
{
    public class RegistrationGraph : IContainerConfigurator
    {
        #region Fields

        private readonly RegistrationFamilyCollection _registrations;
        private readonly List<Registry> _registries;
        private readonly List<AssemblyScanner> _scanners;
        private readonly WeakReference<TypePool> _types;

        #endregion Fields

        #region Properties

        public TypePool Types { get { return _types.Value; } }

        public List<Registry> Registries
        {
            get { return _registries; }
        }

        public RegistrationFamilyCollection RegistrationFamilies
        {
            get { return _registrations; }
        }

        #endregion Properties

        #region c'tor

        public RegistrationGraph()
        {
            _registrations = new RegistrationFamilyCollection();
            _registries = new List<Registry>();
            _scanners = new List<AssemblyScanner>();
            _types = new WeakReference<TypePool>(() => new TypePool(this));
        }

        #endregion c'tor

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

            _registries.Fill(registry);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");
            
            foreach (AssemblyScanner scanner in _scanners)
            {
                scanner.ScanForAll(this);
            }

            foreach(RegistrationFamily family in _registrations)
            {
                family.Configure(container);
            }
        }

        public void AddScanner(AssemblyScanner scanner)
        {
            Guard.AssertNotNull(scanner, "scanner");

            _scanners.Fill(scanner);
        }
    }
}
