using System;
using System.Linq;

using TecX.Common;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Conventions
{
    public class FirstInterfaceConvention : IRegistrationConvention
    {
        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            if (!type.IsConcrete() ||
                !type.CanBeCreated())
            {
                return;
            }

            Type interfaceType = type.AllInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                builder.For(interfaceType).Add(type).Named(type.FullName);
            }
        }
    }
}
