namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Test.TestObjects;
    using TecX.Unity.Tracking;

    [TestClass]
    public class TrackingExtensionFixture
    {
        [TestMethod]
        public void CanResolveLogWithParentType()
        {
            var container = new UnityContainer();

            container.AddNewExtension<TrackingExtension>();

            container.RegisterType<ILog>(
                new InjectionFactory((ctr, type, name) =>
                    {
                        var tracker = ctr.Resolve<ITracker>();

                        var parentType = tracker.CurrentBuildNode.Parent.BuildKey.Type;

                        return LogManager.GetLogger(parentType);
                    }));

            var sut = container.Resolve<UsesLog>();

            Assert.AreEqual(typeof(UsesLog), sut.Log.Type);
        }
    }
}
