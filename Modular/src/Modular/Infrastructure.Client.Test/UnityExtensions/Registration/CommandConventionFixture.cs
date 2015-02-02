namespace Infrastructure.Client.Test.UnityExtensions.Registration
{
    using System;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.UnityExtensions.Registration;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CommandConventionFixture
    {
        [TestMethod]
        public void Should_Register_Command()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new CommandsConvention();

            convention.RegisterOnMatch(container.Object, typeof(NullCommand));

            container.Verify(
                c => c.RegisterType(
                    typeof(ICommand),
                    typeof(NullCommand),
                    "nullCommand",
                    null,
                    It.Is<InjectionMember[]>(im => im.Length == 0)),
                Times.Once);
        }

        [TestMethod]
        public void Should_Not_Register_Non_Command()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new CommandsConvention();

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
