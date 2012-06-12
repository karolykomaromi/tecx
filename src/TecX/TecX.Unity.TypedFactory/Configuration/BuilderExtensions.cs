namespace TecX.Unity.TypedFactory.Configuration
{
    using TecX.Common;
    using TecX.Unity.Configuration.Builders;

    public static class BuilderExtensions
    {
        public static TypeRegistrationBuilder UseFactory(this RegistrationFamilyBuilder family)
        {
            Guard.AssertNotNull(family, "family");

            TypeRegistrationBuilder builder = family.Use(family.From);

            builder.Enrich(members => members.Add(new TypedFactory()));

            return builder;
        }

        public static TypeRegistrationBuilder UseFactory(this RegistrationFamilyBuilder family, ITypedFactoryComponentSelector selector)
        {
            Guard.AssertNotNull(family, "family");
            Guard.AssertNotNull(selector, "selector");

            TypeRegistrationBuilder builder = family.Use(family.From);

            builder.Enrich(members => members.Add(new TypedFactory(selector)));

            return builder;
        }
    }
}
