using System;
using System.Collections.Generic;
using System.Reflection;

namespace TecX.Unity.Registration
{
    public interface IRegistry
    {
        IRegistry ApplyAutoRegistrations();

        IRegistry Exclude(Filter<Assembly> filter);
        IRegistry Exclude(Filter<Type> filter);

        IRegistry LoadAssemblies(Func<IEnumerable<Assembly>> assemblyLoader);
        IRegistry LoadAssembly(Func<Assembly> assemblyLoader);
        
        void AddRegistration(Registration registration);
    }
}
