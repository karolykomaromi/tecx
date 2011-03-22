using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Extensions
{
    public static class UnityContainerExtensions
    {
        public static void Configure(this IUnityContainer container, Action<Registry> action)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(action, "action");

            Registry registry = new Registry();

            action(registry);

            container.AddExtension(registry);
        }
    }
}
