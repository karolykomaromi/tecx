using System;
using System.Collections.Generic;
using System.Linq;

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

        private readonly RegistrationFamilyCollection _registrationFamilies;
        private readonly List<Registry> _registries;
        private readonly List<AssemblyScanner> _scanners;
        private readonly WeakReference<TypePool> _types;

        #endregion Fields

        #region Properties

        public TypePool Types
        {
            get { return _types.Value; }
        }

        public List<Registry> Registries
        {
            get { return _registries; }
        }

        public RegistrationFamilyCollection RegistrationFamilies
        {
            get { return _registrationFamilies; }
        }

        #endregion Properties

        #region c'tor

        public RegistrationGraph()
        {
            this._registrationFamilies = new RegistrationFamilyCollection();
            _registries = new List<Registry>();
            _scanners = new List<AssemblyScanner>();
            _types = new WeakReference<TypePool>(() => new TypePool(this));
        }

        #endregion c'tor

        public void AddScanner(AssemblyScanner scanner)
        {
            Guard.AssertNotNull(scanner, "scanner");

            _scanners.Fill(scanner);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            foreach (AssemblyScanner scanner in _scanners)
            {
                scanner.ScanForAll(this);
            }

            foreach (RegistrationFamily family in this._registrationFamilies)
            {
                foreach (Registration registration in family)
                {
                    registration.Configure(container);
                }
            }
        }

        public RegistrationFamily FindFamily(Type pluginType)
        {
            return RegistrationFamilies[pluginType];
        }

        public void ImportRegistry(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (Registries.Any(x => x.GetType() == type))
            {
                return;
            }

            var registry = (Registry)Activator.CreateInstance(type);

            registry.ConfigureRegistrationGraph(this);
        }
    }
}
