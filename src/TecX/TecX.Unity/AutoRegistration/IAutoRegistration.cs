using System;
using System.Collections.Generic;
using System.Reflection;

namespace TecX.Unity.AutoRegistration
{
    public interface IAutoRegistration
    {
        IAutoRegistration ApplyAutoRegistrations();

        IAutoRegistration Exclude(Filter<Assembly> filter);
        IAutoRegistration Exclude(Filter<Type> filter);

        IAutoRegistration LoadAssemblies(Func<IEnumerable<Assembly>> assemblyLoader);
        IAutoRegistration LoadAssembly(Func<Assembly> assemblyLoader);
        
        void AddRegistration(Registration registration);
    }
}
