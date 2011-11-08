namespace TecX.Unity.Groups
{
    using Microsoft.Practices.Unity;

    public static class UnityContainerExtensions
    {
        public static ISemanticGroup RegisterGroup<TFrom, TTo>(this IUnityContainer container, string name)
        {
            var extension = container.Configure<ISemanticGroupConfigurator>();

            return extension.RegisterGroup<TFrom, TTo>(name);
        }
    }
}