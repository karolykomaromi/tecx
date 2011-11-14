namespace TecX.Unity.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Common.Extensions.Collections;
    using TecX.Unity.Configuration.Common;
    using TecX.Unity.Configuration.Conventions;

    public class Configuration : IContainerConfigurator
    {
        private readonly RegistrationFamilyCollection registrationFamilies;
        private readonly List<ConfigurationBuilder> builders;
        private readonly List<AssemblyScanner> scanners;
        private readonly WeakReference<TypePool> types;
        private readonly List<Action<IUnityContainer>> modifications;

        public Configuration()
        {
            this.registrationFamilies = new RegistrationFamilyCollection();
            this.builders = new List<ConfigurationBuilder>();
            this.scanners = new List<AssemblyScanner>();
            this.types = new WeakReference<TypePool>(() => new TypePool(this));
            this.modifications = new List<Action<IUnityContainer>>();
        }

        public TypePool Types
        {
            get { return this.types.Value; }
        }

        public List<ConfigurationBuilder> Builders
        {
            get { return this.builders; }
        }

        public void AddScanner(AssemblyScanner scanner)
        {
            Guard.AssertNotNull(scanner, "scanner");

            this.scanners.Fill(scanner);
        }

        public void AddModification(Action<IUnityContainer> modification)
        {
            Guard.AssertNotNull(modification, "modification");

            this.modifications.Add(modification);
        }

        public RegistrationFamily FindFamily(Type pluginType)
        {
            return this.registrationFamilies[pluginType];
        }

        public void ImportBuilder(Type type)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertCondition(typeof(ConfigurationBuilder).IsAssignableFrom(type), type, "type");

            if (this.Builders.Any(x => x.GetType() == type))
            {
                return;
            }

            var builder = (ConfigurationBuilder)Activator.CreateInstance(type);

            builder.BuildUp(this);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            // don't use foreach! when adding additional config builders
            // via FindConfigBuildersConvention they might add scanners as well
            // which would cause problems with modified enumeration
            for (int i = 0; i < this.scanners.Count; i++)
            {
                this.scanners[i].ScanForAll(this);
            }

                foreach (AssemblyScanner scanner in this.scanners)
                {
                    scanner.ScanForAll(this);
                }

            this.registrationFamilies.Configure(container);

            foreach (var modification in this.modifications)
            {
                modification(container);
            }
        }
    }
}
