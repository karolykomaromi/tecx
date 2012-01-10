namespace TecX.Unity.Groups
{
    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static ISemanticGroup RegisterGroup<TFrom, TTo>(this IUnityContainer container, string name)
        {
            Guard.AssertNotNull(container, "container");

            var extension = container.Configure<ISemanticGroupConfigurator>();

            return extension.RegisterAsGroup<TFrom, TTo>(name);
        }
    }
}