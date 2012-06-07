namespace TecX.Unity.TypedFactory.Configuration
{
    using TecX.Common;
    using TecX.Unity.Configuration.Builders;

    public static class BuilderExtensions
    {
        public static TypeRegistrationBuilder AsFactory(this RegistrationFamilyBuilder builder)
        {
            Guard.AssertNotNull(builder, "expression");

            var x = builder.Use(builder.From);

            x.Enrich(im => im.Add(new TypedFactory()));

            return x;
        }

        public static TypeRegistrationBuilder AsFactory(this RegistrationFamilyBuilder builder, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(builder, "expression");
            Guard.AssertNotNull(selector, "selector");

            var x = builder.Use(builder.From);

            x.Enrich(im => im.Add(new TypedFactory(selector)));

            return x;
        }
    }
}
