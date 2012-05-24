namespace TecX.Unity.Mapping
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class DefaultMappingExtension : UnityContainerExtension
    {
        private readonly Dictionary<Type, NamedTypeBuildKey> mappings;

        public DefaultMappingExtension()
        {
            this.mappings = new Dictionary<Type, NamedTypeBuildKey>();
        }

        protected override void Initialize()
        {
            DefaultMappingStrategy strategy = new DefaultMappingStrategy(this.mappings);

            this.Context.Strategies.Add(strategy, UnityBuildStage.TypeMapping);

            this.Context.Registering += this.OnRegistering;
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            // a named mapping is registered, the source type of the mapping is either an interface or an abstract class
            // and we don't already have a mapping registered for the source type of the mapping
            if (!string.IsNullOrEmpty(e.Name) &&
                !this.mappings.ContainsKey(e.TypeFrom) &&
                (e.TypeFrom.IsInterface || e.TypeFrom.IsAbstract))
            {
                this.mappings[e.TypeFrom] = new NamedTypeBuildKey(e.TypeTo, e.Name);
            }
        }
    }
}