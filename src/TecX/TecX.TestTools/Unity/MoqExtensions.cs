namespace TecX.TestTools.Unity
{
    using Microsoft.Practices.Unity;

    using Moq;

    using TecX.Common;

    public static class MoqExtensions
    {
        public static Mock<T> Mock<T>(this IUnityContainer container)
            where T : class
        {
            Guard.AssertNotNull(container, "container");

            if (!container.IsRegistered<Mock<T>>())
            {
                AutoMockingBuilderStrategy.RegisterMockInstanceWithContainer(container, typeof(T));
            }

            return container.Resolve<Mock<T>>();
        }
    }
}
