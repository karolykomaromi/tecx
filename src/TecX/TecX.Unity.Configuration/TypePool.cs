using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TecX.Common;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Conventions;

namespace TecX.Unity.Configuration
{
    public class TypePool
    {
        private readonly Cache<Assembly, Type[]> _types;

        public TypePool(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            _types.OnMissing = assembly =>
            {
                try
                {
                    return assembly.GetExportedTypes();
                }
                catch (Exception ex)
                {
                    //graph.Log.RegisterError(170, ex, assembly.FullName);
                    return new Type[0];
                }
            };

            _types = new Cache<Assembly, Type[]>();
        }

        public IEnumerable<Type> For(IEnumerable<Assembly> assemblies, CompositeFilter<Type> filter)
        {
            Guard.AssertNotNull(assemblies, "assemblies");
            Guard.AssertNotNull(filter, "filter");

            return assemblies.SelectMany(x => _types[x].Where(filter.Matches));
        }
    }
}
