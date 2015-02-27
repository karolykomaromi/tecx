namespace Hydra.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AllTypes
    {
        public static IEnumerable<Type> FromHydraAssemblies()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies() 
                   where assembly.FullName.StartsWith("Hydra", StringComparison.OrdinalIgnoreCase) 
                   from type in assembly.ExportedTypes 
                   select type;
        }
    }
}