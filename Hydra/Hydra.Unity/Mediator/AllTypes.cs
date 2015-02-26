namespace Hydra.Unity.Mediator
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class AllTypes
    {
        public static IEnumerable<Type> FromHydraAssemblies()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Hydra", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (Type type in assembly.ExportedTypes)
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}