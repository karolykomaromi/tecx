namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public static class UnityContainerExtensions
    {
        public static ISemanticGroup RegisterGroup<TFrom, TTo>(this IUnityContainer container, string name)
        {
            return RegisterGroup(container, typeof(TFrom), typeof(TTo), name);
        }

        public static ISemanticGroup RegisterGroup(this IUnityContainer container, Type from, Type to, string name)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            var semantic = container.Configure<ISemanticGroupConfigurator>();

            return semantic.RegisterAsGroup(from, to, name);
        }
    }
}