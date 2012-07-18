namespace TecX.Unity.Literals
{
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer WithDefaultConventionsForLiteralParameters(this IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            ILiteralParameters parameters = container.Configure<ILiteralParameters>();

            if (parameters == null)
            {
                var extension = new LiteralParametersExtension();

                container.AddExtension(extension);

                parameters = extension;
            }

            parameters.AddConvention(new ConnectionStringConvention());
            parameters.AddConvention(new AppSettingsConvention());

            return container;
        }

        public static IUnityContainer WithConventionForLiteralParameters(this IUnityContainer container, IDependencyResolverConvention convention)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(convention, "convention");

            ILiteralParameters parameters = container.Configure<ILiteralParameters>();

            if (parameters == null)
            {
                var extension = new LiteralParametersExtension();

                container.AddExtension(extension);

                parameters = extension;
            }

            parameters.AddConvention(convention);

            return container;
        }
    }
}
