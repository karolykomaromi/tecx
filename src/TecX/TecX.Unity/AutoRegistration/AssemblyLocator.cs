using System;
using System.Collections.Generic;
using System.Reflection;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    public abstract class AssemblyLocator
    {
        public abstract IEnumerable<Assembly> GetAssemblies();

        public static implicit operator Func<IEnumerable<Assembly>>(AssemblyLocator locator)
        {
            Guard.AssertNotNull(locator, "locator");

            return locator.GetAssemblies;
        }
    }
}
