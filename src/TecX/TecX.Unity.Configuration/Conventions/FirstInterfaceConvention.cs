using System;
using System.Linq;

using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class FirstInterfaceConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry configuration)
        {
            if (!type.IsConcrete() ||
                !type.CanBeCreated())
            {
                return;
            }

            Type interfaceType = type.AllInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                //configuration.AddType(interfaceType, type);
            }
        }
    }
}
