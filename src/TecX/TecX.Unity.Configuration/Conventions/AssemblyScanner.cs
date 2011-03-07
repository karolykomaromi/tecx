using System;
using System.Collections.Generic;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.Configuration.Conventions
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly List<Assembly> _assemblies;
        private readonly List<IRegistrationConvention> _conventions;
        private readonly CompositeFilter<Type> _filter;

        public AssemblyScanner()
        {
            _assemblies = new List<Assembly>();
            _conventions = new List<IRegistrationConvention>();
            _filter = new CompositeFilter<Type>();
        }

        internal void ScanForAll(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            var registry = new Registry();

            graph.Types.For(_assemblies, _filter).Each(
                type =>
                {
                    _conventions.Each(c => c.Process(type, registry));
                });

            registry.ConfigureRegistrationGraph(graph);

            //_postScanningActions.Each(x => x(pluginGraph));
        }
    }
}