namespace TecX.Unity.Injection
{
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static IUnityContainer WithConstructorArgumentMatchingConvention(this IUnityContainer container, ArgumentMatchingConvention convention)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(convention, "convention");

            ConstructorArgumentMatchingConventions conventions = container.Configure<ConstructorArgumentMatchingConventions>();

            if (conventions == null)
            {
                conventions = new ConstructorArgumentMatchingConventions();

                container.AddExtension(conventions);
            }

            conventions.Add(convention);

            return container;
        }
    }
}