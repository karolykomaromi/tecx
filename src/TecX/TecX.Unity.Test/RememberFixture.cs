namespace TecX.Unity.Test
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Unity.Test.TestObjects;

    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RememberFixture
    {
        [TestMethod]
        public void TestMethod1()
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

        class  One : IFoo { }

        class Two : IFoo { }

        class Three : IFoo { }
    }

    public class Remember : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new RememberStrategy();

            this.Context.Strategies.Add(strategy, UnityBuildStage.Setup);
        }
    }

    public class RememberStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
        }
    }
}
