namespace Infrastructure.Client.Test.UnityExtensions.Registration
{
    using System;
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.Options;
    using Infrastructure.UnityExtensions.Registration;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class OptionsConventionFixture
    {
        [TestMethod]
        public void Should_Register_Options()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new OptionsConvention();

            convention.RegisterOnMatch(container.Object, typeof(MyOptions));

            container.Verify(
                c => c.RegisterType(
                    typeof(IOptions),
                    typeof(MyOptions),
                    "myOptions",
                    It.Is<LifetimeManager>(lm => lm is ContainerControlledLifetimeManager),
                    It.Is<InjectionMember[]>(im => im.Length == 0)),
                Times.Once);
        }

        [TestMethod]
        public void Should_Not_Register_Non_Options()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new OptionsConvention();

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
