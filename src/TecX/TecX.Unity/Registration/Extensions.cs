using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public static class Extensions
    {
        public static IRegistry ConfigureRegistrations(this IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            return new RegistryBuilder(container);
        }
    }
}