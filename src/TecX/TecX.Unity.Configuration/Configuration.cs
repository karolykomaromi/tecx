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
    public class Configuration : IContainerConfigurator
    {
        private readonly RegistrationFamilyCollection _registrationFamilies;
        private readonly List<ConfigurationBuilder> _builders;
        private readonly List<AssemblyScanner> _scanners;
        private readonly WeakReference<TypePool> _types;

        public TypePool Types
        {
            get { return _types.Value; }
        }

        public List<ConfigurationBuilder> Builders
        {
            get { return this._builders; }
        }

        public RegistrationFamilyCollection RegistrationFamilies
        {
            get { return _registrationFamilies; }
        }

        public Configuration()
        {
            _registrationFamilies = new RegistrationFamilyCollection();
            this._builders = new List<ConfigurationBuilder>();
            _scanners = new List<AssemblyScanner>();
            _types = new WeakReference<TypePool>(() => new TypePool(this));
        }

        public void AddScanner(AssemblyScanner scanner)
        {
            Guard.AssertNotNull(scanner, "scanner");

            _scanners.Fill(scanner);
        }

        public RegistrationFamily FindFamily(Type pluginType)
        {
            return RegistrationFamilies[pluginType];
        }

        public void ImportRegistry(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (this.Builders.Any(x => x.GetType() == type))
            {
                return;
            }

            var registry = (ConfigurationBuilder)Activator.CreateInstance(type);

            registry.BuildUp(this);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            foreach (AssemblyScanner scanner in _scanners)
            {
                scanner.ScanForAll(this);
            }

            _registrationFamilies.Configure(container);
        }
    }
}
