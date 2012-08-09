namespace TecX.Unity.Configuration.Conventions
{
    using System;

    using TecX.Common;
    using TecX.Common.Reflection;

    /// <summary>
    /// Registers implementations that match the ISomething -> Something pattern
    /// </summary>
    public class ImplementsIInterfaceNameConvention : IRegistrationConvention
    {
        public void Process(Type type, ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotNull(builder, "ConfigurationBuilder");

            if (!type.IsConcrete())
            {
                return;
            }

            Type pluginType = FindPluginType(type);
            if (pluginType != null && 
                Constructor.HasConstructors(type))
            {
                builder.For(pluginType).Add(type).Named(type.FullName);
            }
        }

        private static Type FindPluginType(Type concreteType)
        {
            string interfaceName = "I" + concreteType.Name;
            Type[] interfaces = concreteType.GetInterfaces();
            return Array.Find(interfaces, t => t.Name == interfaceName);
        }
    }
}
