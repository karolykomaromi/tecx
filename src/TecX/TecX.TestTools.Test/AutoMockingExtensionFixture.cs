namespace TecX.TestTools.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.TestTools.Test.TestObjects;
    using TecX.TestTools.Unity;

    [TestClass]
    public class AutoMockingExtensionFixture
    {
        [TestMethod]
        public void CanTurnOnAutoMocking()
        {
            var container = new UnityContainer();

            container.AddNewExtension<AutoMockingExtension>();

            var foo = container.Resolve<IFoo>();

            Assert.IsNotNull(foo);
        }

        [TestMethod]
        public void CanConfigureMock()
        {
            var container = new UnityContainer();

            var mock = container.Mock<IFoo>();

            mock.SetupGet(f => f.Bar).Returns("1");

            IFoo foo = container.Resolve<IFoo>();

            Assert.AreEqual("1", foo.Bar);
        }
    }
}
