namespace TecX.Unity.Configuration.Conventions
{
    using System;
    using System.Linq;

    using TecX.Common;

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
