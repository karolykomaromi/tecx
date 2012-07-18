namespace TecX.TestTools.Unity
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    using Moq;

    using TecX.Common;
    using TecX.Common.Extensions.Error;

    public class AutoMockingBuilderStrategy : BuilderStrategy
    {
        private readonly IUnityContainer container;

        public AutoMockingBuilderStrategy(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            this.container = container;
        }

        public static Mock RegisterMockInstanceWithContainer(IUnityContainer container, Type typeToMock)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(typeToMock, "typeToMock");

            if (!typeToMock.IsInterface)
            {
                string msg = string.Format("'typeToMock' ({0}) is not an interface.", typeToMock.FullName);
                throw new ArgumentException(msg)
                    .WithAdditionalInfo("typeToMock", typeToMock);
            }

            var genericMockType = typeof(Mock<>).MakeGenericType(typeToMock);

            Mock mock = (Mock)Activator.CreateInstance(genericMockType);

            container.RegisterInstance(genericMockType, mock);
            container.RegisterInstance(typeToMock, mock.Object);

            return mock;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            var type = context.OriginalBuildKey.Type;

            if (type.IsInterface && !this.container.IsRegistered(type))
            {
                Mock mock = RegisterMockInstanceWithContainer(this.container, type);

                context.Existing = mock.Object;
            }
        }
    }
}