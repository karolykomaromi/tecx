using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.ContextualBinding;
using TecX.Unity.ContextualBinding.Test.TestObjects;

using TecX.TestTools;

namespace TecX.Unity.Test
{
    public abstract class Given_ContainerWithContextualBindingExtension : GivenWhenThen
    {
        protected IUnityContainer container;

        protected override void Given()
        {
            container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();
        }
    }

    [TestClass]
    public class When_ContextMatchesPredicate : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true);
        }

        [TestMethod]
        public void Then_ResolvesProperly()
        {
            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.IsNotNull(myClass);
        }
    }

    [TestClass]
    public class When_ContextDoesNotMatchPredicate : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => false);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void Then_Throws()
        {
            container.Resolve<IMyInterface>();
        }
    }
    
    [TestClass]
    public class When_MappingWithNonTransientLifetimeManager : Given_ContainerWithContextualBindingExtension
    {
        private IMyInterface first, second;

        protected override void When()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true,
                new ContainerControlledLifetimeManager());

            first = container.Resolve<IMyInterface>();

            second = container.Resolve<IMyInterface>();
        }

        [TestMethod]
        public void Then_ResolvesWithCorrectLifetime()
        {
            Assert.AreSame(first, second);
        }
    }
    
    [TestClass]
    public class When_MappingWithInjectionMember : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyClass>((bindingContext, builderContext) => true, new InjectionConstructor("c'tor with parameter"));
        }

        [TestMethod]
        public void Then_ResolvesUsingSpecifiedInjectionMember()
        {
            var myClass = container.Resolve<IMyInterface>() as MyClass;

            Assert.IsNotNull(myClass);

            Assert.AreEqual("c'tor with parameter", myClass.Str);
        }
    }

    [TestClass]
    public class When_RegisteringContextualAfterDefaultMapping : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterType<IMyInterface, MyClass>((bindingContext, builderContext) => false);
        }

        [TestMethod]
        public void Then_CanStillResolveDefaultMapping()
        {
            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }
    }

    [TestClass]
    public class When_RegisteringDefaultAfterContextualMapping : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyParameterLessClass>((bindingContext, builderContext) => true);

            container.RegisterType<IMyInterface, MyOtherClass>();
        }

        [TestMethod]
        public void Then_ContextualMappingHasPrecedence()
        {
            MyParameterLessClass myParameterLessClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.IsNotNull(myParameterLessClass);
        }
    }

    [TestClass]
    public class When_ContextualMappingUsesInformationFromCtx : Given_ContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.Configure<IContextualBindingConfiguration>().Put("someKey", 123);

            Predicate<IBindingContext, IBuilderContext> matches = (bindingContext, builderContext) =>
            {
                if ((int)bindingContext["someKey"] == 123)
                    return true;

                return false;
            };

            container.RegisterType<IMyInterface, MyParameterLessClass>(matches);
        }

        [TestMethod]
        public void Then_PullsInformationCorrectlyFromContext()
        {
            var myClass = container.Resolve<BindingNamespaceTest>();

            Assert.IsNotNull(myClass);
        }
    }

    public abstract class Given_Instance : Given_ContainerWithContextualBindingExtension
    {
        protected MyParameterLessClass instance;

        protected override void Given()
        {
            base.Given();

            instance = new MyParameterLessClass();
        }
    }

    [TestClass]
    public class When_RegisteringInstanceWithContext : Given_Instance
    {
        protected override void When()
        {
            container.RegisterInstance<IMyInterface>(instance, (bindingContext, builderContext) => true);
        }

        [TestMethod]
        public void Then_PullsInstanceOnMatch()
        {
            MyParameterLessClass myClass = container.Resolve<IMyInterface>() as MyParameterLessClass;

            Assert.AreSame(instance, myClass);
        }
    }

    [TestClass]
    public class When_RegisteringInstanceWithFailingPredicate : Given_Instance
    {
        protected override void When()
        {
            container.RegisterInstance<IMyInterface>(instance, (bindingContext, builderContext) => false);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void Then_Throws()
        {
            container.Resolve<IMyInterface>();
        }
    }

    [TestClass]
    public class When_RegisteringInstanceAfterDefaultMapping : Given_Instance
    {
        protected override void When()
        {
            container.RegisterType<IMyInterface, MyOtherClass>();

            container.RegisterInstance<IMyInterface>(new MyParameterLessClass(), (bindingContext, builderContext) => false);
        }

        [TestMethod]
        public void Then_CanResolveDefault()
        {
            MyOtherClass myOtherClass = container.Resolve<IMyInterface>() as MyOtherClass;

            Assert.IsNotNull(myOtherClass);
        }
    }

    [TestClass]
    public class When_RegisteringDefaultMappingAfterInstance : Given_Instance
    {
        protected override void When()
        {
            container.RegisterInstance<IMyInterface>(new MyClass(), (bindingContext, builderContext) => true);

            container.RegisterType<IMyInterface, MyOtherClass>();
        }

        [TestMethod]
        public void Then_CanResolveDefault()
        {
            MyClass myClass = container.Resolve<IMyInterface>() as MyClass;

            Assert.IsNotNull(myClass);
        }
    }
}
