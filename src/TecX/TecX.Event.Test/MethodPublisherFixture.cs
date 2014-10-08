namespace TecX.Event.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TecX.Event.Test.TestObjects;

    [TestClass]
    public class MethodPublisherFixture
    {
        [TestMethod]
        public void CanPublish()
        {
            var msg = new Message();

            var mock = new Mock<IEventAggregator>();

            var publisher = new MessagePublisher(mock.Object);

            publisher.Publish(msg);

            mock.Verify(ea => ea.Publish<Message>(It.IsAny<Message>()));
        }
    }
}
