using Microsoft.Practices.Unity;

namespace TecX.Unity.Registration
{
    public static class The
    {
        public static ContainerExtensionOptionsBuilder Extension<TExtension>()
            where TExtension : UnityContainerExtension
        {
            return new ContainerExtensionOptionsBuilder(typeof(TExtension));
        }
    }
}