using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.ContextualBinding;
using TecX.Unity.ContextualBinding.Test.TestObjects;

namespace TecX.Unity.Test
{
    [TestClass]
    public class ContextualBindingFixture
    {
        [TestMethod]
        public void GivenContainer_WhenRegisteringWithContext_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true);

            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.IsNotNull(myClass);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void GivenContainer_WhenRegisteringWithContext_FailsIfPredicateFails()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyClass>((bindingContext, builderContext) => false);

            container.Resolve<IMyInterface>();
        }

        [TestMethod]
        public void GivenMappingWithNonTransientLifetimeManager_WhenResolving_ResolvesCorrectLifetime()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true,
                new ContainerControlledLifetimeManager());

            var first = container.Resolve<IMyInterface>();

            var second = container.Resolve<IMyInterface>();

            Assert.AreSame(first, second);
        }

        [TestMethod]
        public void GivenMappingWithInjectionMember_WhenResolving_ResolvesUsingSpecifiedInjectionMember()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyClass>((bindingContext, builderContext) => true, new InjectionConstructor("c'tor with parameter"));

            var myClass = container.Resolve<IMyInterface>() as MyClass;

            Assert.IsNotNull(myClass);

            Assert.AreEqual("c'tor with parameter", myClass.Str);
        }

        [TestMethod]
        public void GivenRegistrationWithContextAfterDefaultRegistration_WhenResolving_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterType<IMyInterface, MyClass>((bindingContext, builderContext) => false);

            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }

        [TestMethod]
        public void GivenDefaultRegistrationAfterRegistrationWithContext_WhenResolving_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true);

            container.RegisterType<IMyInterface, MyOtherClass>();

            MyParameterLessClass myParameterLessClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.IsNotNull(myParameterLessClass);
        }

        [TestMethod]
        public void GivenRegistrationThatUsesContextInformation_WhenResolving_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.Configure<IContextualBindingConfiguration>().Put("someKey", 123);

            Predicate<IBindingContext, IBuilderContext> matches = (bindingContext, builderContext) =>
            {
                if ((int)bindingContext["someKey"] == 123)
                    return true;

                return false;
            };

            container.RegisterType<IMyInterface, MyParameterLessClass>(matches);

            var myClass = container.Resolve<BindingNamespaceTest>();

            Assert.IsNotNull(myClass);
        }

        [TestMethod]
        public void GivenContainer_WhenRegisteringInstanceWithContext_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            MyParameterLessClass instance = new MyParameterLessClass();

            container.RegisterInstance<IMyInterface>(instance, (bindingContext, builderContext) => true);

            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.AreSame(instance, myClass);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void GivenContainer_WhenRegisteringInstanceWithContext_FailsIfPredicateFails()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            var instance = new MyParameterLessClass();

            container.RegisterInstance<IMyInterface>(instance, (bindingContext, builderContext) => false);

            container.Resolve<IMyInterface>();
        }

        [TestMethod]
        public void GivenInstanceRegistrationWithContextAfterDefaultRegistration_WhenResolving_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterInstance<IMyInterface>(new MyParameterLessClass(), (bindingContext, builderContext) => false);

            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }

        [TestMethod]
        public void GivenDefaultRegistrationAfterInstanceRegistrationWithContext_WhenResolving_ResolvesProperly()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();

            container.RegisterInstance<IMyInterface>(new MyClass(), (bindingContext, builderContext) => true);

            container.RegisterType<IMyInterface, MyOtherClass>();

            MyClass myClass = container.Resolve<IMyInterface>() as MyClass;

            Assert.IsNotNull(myClass);
        }
    }
}
