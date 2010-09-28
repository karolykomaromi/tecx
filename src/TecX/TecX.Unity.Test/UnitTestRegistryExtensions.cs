using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TecX.Unity.Registration;
using TecX.Common;
using System.Reflection;

namespace TecX.Unity.Test
{
    static class UnitTestRegistryExtensions
    {
        public static IRegistry ExcludeUnitTestAssemblies(this IRegistry registry)
        {
            Guard.AssertNotNull(registry, "registry");

            registry.Exclude(new Filter<Assembly>(a =>
                a.GetName().Name.StartsWith("Microsoft.VisualStudio", StringComparison.OrdinalIgnoreCase),
                "Exclude system assemblies."));

            return registry;
        }
    }
}
