namespace TecX.EnumClasses.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.EnumClasses.Tests.TestObjects;

    [TestClass]
    public class ServiceFixture
    {
        [TestMethod]
        public void Should_SortAscending()
        {
            var svc = new SortingService();

            IEnumerable<SerializeMe> sms = new[] { new SerializeMe { Text = "1" }, new SerializeMe { Text = "3" }, new SerializeMe { Text = "2" } };

            var result = svc.Sort(sms, SortOrder.Ascending).ToList();

            Assert.AreEqual("1", result[0].Text);
            Assert.AreEqual("2", result[1].Text);
            Assert.AreEqual("3", result[2].Text);
        }

        [TestMethod]
        public void Should_SortDescending()
        {
            var svc = new SortingService();

            IEnumerable<SerializeMe> sms = new[] { new SerializeMe { Text = "1" }, new SerializeMe { Text = "3" }, new SerializeMe { Text = "2" } };

            var result = svc.Sort(sms, SortOrder.Descending).ToList();

            Assert.AreEqual("1", result[2].Text);
            Assert.AreEqual("2", result[1].Text);
            Assert.AreEqual("3", result[0].Text);
        }

        [TestMethod]
        public void Should_TransparentlyConvert_BetweenSimpleEnumOnClient_AndComplexEnumClassOnServer()
        {
            IEnumerable<SerializeMe2> sms = new[] { new SerializeMe2 { Text = "1" }, new SerializeMe2 { Text = "3" }, new SerializeMe2 { Text = "2" } };

            using (ServiceHost host = new ServiceHost(typeof(SortingService), new Uri("http://localhost:12345/svc")))
            {
                host.Open();

                var factory = new ChannelFactory<ISortingServiceClient>(new BasicHttpBinding());

                var proxy = factory.CreateChannel(new EndpointAddress("http://localhost:12345/svc"));

                var result = proxy.Sort(sms, SortOrderEnum.Ascending).ToList();

                Assert.AreEqual("1", result[0].Text);
                Assert.AreEqual("2", result[1].Text);
                Assert.AreEqual("3", result[2].Text);
            }
        }
    }
}
