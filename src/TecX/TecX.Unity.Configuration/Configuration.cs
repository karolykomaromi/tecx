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
        private readonly List<Action<IUnityContainer>> _modifications;

        public TypePool Types
        {
            get { return _types.Value; }
        }

        public List<ConfigurationBuilder> Builders
        {
            get { return _builders; }
        }
        
        public Configuration()
        {
            _registrationFamilies = new RegistrationFamilyCollection();
            _builders = new List<ConfigurationBuilder>();
            _scanners = new List<AssemblyScanner>();
            _types = new WeakReference<TypePool>(() => new TypePool(this));
            _modifications = new List<Action<IUnityContainer>>();
        }

        public void AddScanner(AssemblyScanner scanner)
        {
            Guard.AssertNotNull(scanner, "scanner");

            _scanners.Fill(scanner);
        }

        public void AddModification(Action<IUnityContainer> modification)
        {
            Guard.AssertNotNull(modification, "modification");

            _modifications.Add(modification);
        }

        public RegistrationFamily FindFamily(Type pluginType)
        {
            return _registrationFamilies[pluginType];
        }

        public void ImportBuilder(Type type)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertCondition(typeof(ConfigurationBuilder).IsAssignableFrom(type), type, "type");

            if (Builders.Any(x => x.GetType() == type))
            {
                return;
            }

            var builder = (ConfigurationBuilder)Activator.CreateInstance(type);

            builder.BuildUp(this);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            for (int i = 0; i < _scanners.Count; i++)
            {
                _scanners[i].ScanForAll(this);
            }

                foreach (AssemblyScanner scanner in _scanners)
                {
                    scanner.ScanForAll(this);
                }

            _registrationFamilies.Configure(container);

            foreach (var modification in _modifications)
            {
                modification(container);
            }
        }
    }
}
