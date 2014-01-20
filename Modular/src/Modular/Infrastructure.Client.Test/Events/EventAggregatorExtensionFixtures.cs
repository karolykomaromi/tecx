namespace Infrastructure.Client.Test.Events
{
    using Infrastructure.Events;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EventAggregatorExtensionFixtures
    {
        [TestMethod]
        public void Should_Auto_Subscribe_ViewModel()
        {
            IUnityContainer container = new UnityContainer();

            container.AddNewExtension<EventAggregatorExtension>();
        }
    }
}
