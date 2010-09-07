﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace TecX.Unity.AutoRegistration
{
    public interface IAutoRegistration
    {
        IAutoRegistration ApplyAutoRegistrations();

        IAutoRegistration Exclude(Filter<Assembly> filter);
        IAutoRegistration Exclude(Filter<Type> filter);
        IAutoRegistration ExcludeSystemAssemblies();

        IAutoRegistration Include(Filter<Type> filter, RegistrationOptionsBuilder builder);

        IAutoRegistration LoadAssemblies(Func<IEnumerable<Assembly>> assemblyLoader);
        IAutoRegistration LoadAssembly(Func<Assembly> assemblyLoader);

        IAutoRegistration Include(Filter<Type> filter, InterceptionOptionsBuilder builder);

        IAutoRegistration EnableInterception();
    }
}
