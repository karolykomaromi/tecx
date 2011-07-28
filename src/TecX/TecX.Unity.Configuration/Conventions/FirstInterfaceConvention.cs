using System;
using System.Linq;

using TecX.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class FirstInterfaceConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(registry, "registry");

            if (!type.IsConcrete() ||
                !type.CanBeCreated())
            {
                return;
            }

            Type interfaceType = type.AllInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                registry.AddType(interfaceType, type, type.FullName);
            }
        }
    }
}
