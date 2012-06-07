namespace TecX.ServiceModel.Test
{
    using System.ServiceModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.ServiceModel.Test.TestClasses;
    using TecX.ServiceModel.Unity.AutoMagic;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WcfAutoMagicFixture
    {
        [TestMethod]
        [TestCategory("LongRunning")]
        public void CanAddDiscoveryBehaviorUsingAttribute()
        {
            using (ServiceHost host = new ServiceHost(typeof(SyncService), Constants.BaseAddress))
            {
                BindingDiscoveryServiceBehavior bindingDiscoveryServiceBehavior =
                    host.Description.Behaviors.Find<BindingDiscoveryServiceBehavior>();

                Assert.IsNotNull(bindingDiscoveryServiceBehavior);
            }
        }
    }
}