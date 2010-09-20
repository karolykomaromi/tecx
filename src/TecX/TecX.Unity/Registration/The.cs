using Microsoft.Practices.Unity;

namespace TecX.Unity.Registration
{
    public static class The
    {
        public static ContainerExtensionOptionsBuilder Extension<TExtension>()
            where TExtension : UnityContainerExtension, new()
        {
            return new ContainerExtensionOptionsBuilder(() => new TExtension());
        }
    }
}