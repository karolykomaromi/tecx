namespace Infrastructure.Client.Test.UnityExtensions.Registration
{
    using System;
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.UnityExtensions.Registration;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ViewModelConventionFixture
    {
        [TestMethod]
        public void Should_Register_Type_Derived_From_ViewModel()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new ViewModelConvention();

            convention.RegisterOnMatch(container.Object, typeof(ViewModel1));

            container.Verify(
                c => c.RegisterType(
                    null,
                    typeof(ViewModel1),
                    null,
                    null,
                    It.Is<InjectionMember[]>(im => im[0] is SmartConstructor)),
                Times.Once);
        }

        [TestMethod]
        public void Should_Not_Register_ViewModel()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new ViewModelConvention();

            convention.RegisterOnMatch(container.Object, typeof(ViewModel));

            container.Verify(
                c => c.RegisterType(
                    It.IsAny<Type>(), 
                    It.IsAny<Type>(), 
                    It.IsAny<string>(), 
                    It.IsAny<LifetimeManager>(), 
                    It.IsAny<InjectionMember[]>()), 
                Times.Never);
        }

        [TestMethod]
        public void Should_Not_Register_Options()
        {
            var container = new Mock<IUnityContainer>();

            IRegistrationConvention convention = new ViewModelConvention();

            convention.RegisterOnMatch(container.Object, typeof(MyOptions));

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
