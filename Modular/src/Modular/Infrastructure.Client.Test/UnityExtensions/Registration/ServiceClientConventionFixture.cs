namespace Infrastructure.Client.Test.UnityExtensions.Registration
{
    using System;
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.UnityExtensions.Registration;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ServiceClientConventionFixture
    {
        [TestMethod]
        public void Should_Register_ServiceClient()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new ServiceClientConvention();

            convention.RegisterOnMatch(container.Object, typeof(DummyServiceClient));

            container.Verify(
                c => c.RegisterType(
                    typeof(IDummyService),
                    typeof(DummyServiceClient),
                    null,
                    null,
                    It.Is<InjectionMember[]>(im => im.Length == 0)),
                Times.Once);
        }

        [TestMethod]
        public void Should_Not_Register_Non_ServiceClient()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new ServiceClientConvention();

            convention.RegisterOnMatch(container.Object, typeof(object));

            container.Verify(
                c => c.RegisterType(
                    It.IsAny<Type>(), 
                    It.IsAny<Type>(), 
                    It.IsAny<string>(), 
                    It.IsAny<LifetimeManager>(), 
                    It.IsAny<InjectionMember[]>()), 
                Times.Never);
        }
    }
}
