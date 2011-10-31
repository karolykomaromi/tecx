namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_UsingLifetimeForFamily : Given_ContainerAndBuilder
    {
        protected override void Given()
        {
            base.Given();

            builder.For<IMyInterface>().Use<MyClass>().AsSingleton();

            builder.For<IMyInterface>().LifetimeIs(() => new TransientLifetimeManager());
        }

        [TestMethod]
        public void Then_CanSetLifetimeForWholeFamily()
        {
            IMyInterface r1 = container.Resolve<IMyInterface>();
            IMyInterface r2 = container.Resolve<IMyInterface>();

            Assert.AreNotSame(r1, r2);
        }
    }
}