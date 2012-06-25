namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static void RegisterGroup(this IUnityContainer container, Action<IUnityContainer> action)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(action, "action");

            using (var proxy = new GroupingProxyContainer(container))
            {
                action(proxy);
            }
        }
    }
}