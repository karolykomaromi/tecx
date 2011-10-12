using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.Configuration.Common
{
    public class TypePool
    {
        private readonly Cache<Assembly, Type[]> _types;

        public TypePool(RegistrationGraph graph)
        {
            Guard.AssertNotNull(graph, "graph");

            _types = new Cache<Assembly, Type[]>(
                assembly =>
                    {
                        try
                        {
                            return assembly.GetExportedTypes();
                        }
                        catch (Exception ex)
                        {
                            return Type.EmptyTypes;
                        }
                    });
        }

        public IEnumerable<Type> For(IEnumerable<Assembly> assemblies, CompositeFilter<Type> filter)
        {
            Guard.AssertNotNull(assemblies, "assemblies");
            Guard.AssertNotNull(filter, "filter");

            //if the cache contains information about the exported types of this assembly it will return those
            //values. otherwise it will use the OnMissing delegate we provided in the constructor to read all exported types
            //for this assembly.
            return assemblies.SelectMany(assembly => _types[assembly].Where(filter.Matches));
        }
    }
}
