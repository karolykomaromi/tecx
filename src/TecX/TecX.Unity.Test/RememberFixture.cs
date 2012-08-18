namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;

    using TecX.Unity.Mapping;
    using TecX.Unity.Test.TestObjects;

    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RememberFixture
    {
        [TestMethod]
        public void CanResolveMultipeDefaultMappingsUsingResolveAll()
        {
            var container = new UnityContainer().AddNewExtension<Remember>();

            container.RegisterType<IFoo, One>();
            container.RegisterType<IFoo, Two>();
            container.RegisterType<IFoo, Three>();

            IFoo[] foos = container.ResolveAll<IFoo>().OrderBy(f => f.GetType().Name).ToArray();

            Assert.AreEqual(3, foos.Length);
            Assert.IsInstanceOfType(foos[0], typeof(One));
            Assert.IsInstanceOfType(foos[1], typeof(Three));
            Assert.IsInstanceOfType(foos[2], typeof(Two));
        }

        [TestMethod]
        public void DefaultForResolveIsLastRegisteredMapping()
        {
            var container = new UnityContainer().AddNewExtension<Remember>();

            container.RegisterType<IFoo, One>();
            container.RegisterType<IFoo, Two>();
            container.RegisterType<IFoo, Three>();

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsInstanceOfType(foo, typeof(Three));

        }

        [TestMethod]
        public void MultipleDefaultMappingsRespectInjectionMembers()
        {
            var container = new UnityContainer().AddNewExtension<Remember>();

            container.RegisterType<IFoo, One>(new InjectionProperty("Bar", "1"));
            container.RegisterType<IFoo, Two>();
            container.RegisterType<IFoo, Three>();

            IFoo[] foos = container.ResolveAll<IFoo>().OrderBy(f => f.GetType().Name).ToArray();

            Assert.AreEqual("1", ((One)foos[0]).Bar);
        }

        [TestMethod]
        public void MultipleDefaultMappingsWorkWithInstances()
        {
            var container = new UnityContainer().AddNewExtension<Remember>();

            IFoo three = new Three();

            container.RegisterType<IFoo, One>();
            container.RegisterType<IFoo, Two>();
            container.RegisterInstance<IFoo>(three);

            IFoo[] foos = container.ResolveAll<IFoo>().OrderBy(f => f.GetType().Name).ToArray();

            var foo = container.Resolve<IFoo>();

            Assert.AreSame(foos[1], foo);
        }

        [TestMethod]
        public void MultipleDefaultMappingsRespectLifeTime()
        {
            var container = new UnityContainer().AddNewExtension<Remember>();

            container.RegisterType<IFoo, One>();
            container.RegisterType<IFoo, Two>();
            container.RegisterType<IFoo, Three>(new ContainerControlledLifetimeManager());

            IFoo[] foos = container.ResolveAll<IFoo>().OrderBy(f => f.GetType().Name).ToArray();

            var foo = container.Resolve<IFoo>();

            Assert.AreSame(foos[1], foo);
        }

        [TestMethod]
        public void ResolveAllIncludesDefaultMapping()
        {
            var container = new UnityContainer().AddNewExtension<Remember>();

            container.RegisterType<IFoo, One>();
            container.RegisterType<IFoo, Two>("1");

            IFoo[] foos = container.ResolveAll<IFoo>().ToArray();

            Assert.AreEqual(2, foos.Length);
        }

        class One : IFoo
        {
            public string Bar { get; set; }
        }

        class Two : IFoo { }

        class Three : IFoo { }
    }
}
