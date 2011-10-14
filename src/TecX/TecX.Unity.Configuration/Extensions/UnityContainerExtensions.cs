using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Configuration.Extensions
{
    public static class UnityContainerExtensions
    {
        public static void Configure(this IUnityContainer container, Action<ConfigurationBuilder> action)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(action, "action");

            ConfigurationBuilder builder = new ConfigurationBuilder();

            action(builder);

            container.AddExtension(builder);
        }
    }
}
