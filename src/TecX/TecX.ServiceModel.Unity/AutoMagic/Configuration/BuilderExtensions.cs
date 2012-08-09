namespace TecX.ServiceModel.Unity.AutoMagic.Configuration
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using TecX.Common;
    using TecX.Unity.Configuration.Builders;

    public static class BuilderExtensions
    {
        public static TypeRegistrationBuilder DiscoverService(this RegistrationFamilyBuilder family)
        {
            Guard.AssertNotNull(family, "family");

            TypeRegistrationBuilder builder = family.Use(family.From);

            builder.Enrich(members => members.Add(new AutoDiscoveryProxyFactory()));

            return builder;
        }

        public static TypeRegistrationBuilder ServiceFromConfig(this RegistrationFamilyBuilder family, string endpointConfigName)
        {
            Guard.AssertNotNull(family, "family");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            TypeRegistrationBuilder builder = family.Use(family.From);

            builder.Enrich(members => members.Add(new ConfigFileProxyFactory(endpointConfigName)));

            return builder;
        }

        public static TypeRegistrationBuilder Service(this RegistrationFamilyBuilder family, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(family, "family");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            TypeRegistrationBuilder builder = family.Use(family.From);

            builder.Enrich(members => members.Add(new SpecifiedProxyFactory(address, binding)));

            return builder;
        }
    }
}
