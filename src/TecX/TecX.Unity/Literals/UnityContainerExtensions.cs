namespace TecX.Unity.Literals
{
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer WithDefaultConventionsForLiteralParameters(this IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            LiteralParameters parameters = container.Configure<LiteralParameters>();

            if (parameters == null)
            {
                parameters = new LiteralParameters();

                container.AddExtension(parameters);
            }

            parameters.AddConvention(new ConnectionStringConvention());
            parameters.AddConvention(new AppSettingsConvention());

            return container;
        }

        public static IUnityContainer WithConventionsForLiteralParameters(this IUnityContainer container, params IDependencyResolverConvention[] conventions)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(conventions, "convention");

            LiteralParameters parameters = container.Configure<LiteralParameters>();

            if (parameters == null)
            {
                parameters = new LiteralParameters();

                container.AddExtension(parameters);
            }

            foreach (var convention in conventions)
            {
                parameters.AddConvention(convention);
            }

            return container;
        }
    }
}
