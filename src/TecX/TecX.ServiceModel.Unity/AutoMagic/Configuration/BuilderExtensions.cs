namespace TecX.ServiceModel.Unity.AutoMagic.Configuration
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using TecX.Common;
    using TecX.Unity.Configuration.Builders;

    public static class BuilderExtensions
    {
        public static TypeRegistrationBuilder DiscoverService(this RegistrationFamilyBuilder builder)
        {
            Guard.AssertNotNull(builder, "expression");

            TypeRegistrationBuilder x = builder.Use(builder.From);

            x.Enrich(im => im.Add(new AutoDiscoveryProxyFactory()));

            return x;
        }

        public static TypeRegistrationBuilder ServiceFromConfig(this RegistrationFamilyBuilder builder, string endpointConfigName)
        {
            Guard.AssertNotNull(builder, "expression");
            Guard.AssertNotEmpty(endpointConfigName, "endpointConfigName");

            TypeRegistrationBuilder x = builder.Use(builder.From);

            x.Enrich(im => im.Add(new ConfigFileProxyFactory(endpointConfigName)));

            return x;
        }

        public static TypeRegistrationBuilder Service(this RegistrationFamilyBuilder builder, EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(builder, "expression");
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            TypeRegistrationBuilder x = builder.Use(builder.From);

            x.Enrich(im => im.Add(new SpecifiedProxyFactory(address, binding)));

            return x;
        }
    }
}
