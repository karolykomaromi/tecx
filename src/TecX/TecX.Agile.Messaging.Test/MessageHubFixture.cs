namespace TecX.Agile.Messaging.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Event;

    [TestClass]
    public class MessageHubFixture
    {
        [TestMethod]
        public void InboundChangePropCmdNotPropagatedAgain()
        {
            var channel = new Mock<IMessageChannel>();
            var ea = new Mock<IEventAggregator>();

            Guid id = Guid.NewGuid();

            var cmd = new ChangeProperty(id, "1", 1, 2);
            var @event = new PropertyChanged(id, "1", 1, 2);

            var hub = new MessageHub(channel.Object, ea.Object);

            ea.Setup(e => e.Publish(It.IsAny<ChangeProperty>())).Callback(() => hub.Handle(@event));

            hub.Handle(cmd);

            channel.Verify(c => c.Send(@event), Times.Never());
            ea.Verify(e => e.Publish(cmd), Times.Once());
        }

        [TestMethod]
        public void InboundHighlightFieldCmdNotPropagatedAgain()
        {
            var channel = new Mock<IMessageChannel>();
            var ea = new Mock<IEventAggregator>();

            Guid id = Guid.NewGuid();

            var cmd = new HighlightField(id, "1");
            var @event = new FieldHighlighted(id, "1");

            var hub = new MessageHub(channel.Object, ea.Object);

            ea.Setup(e => e.Publish(It.IsAny<HighlightField>())).Callback(() => hub.Handle(@event));

            hub.Handle(cmd);

            channel.Verify(c => c.Send(@event), Times.Never());
            ea.Verify(e => e.Publish(cmd), Times.Once());
        }

        [TestMethod]
        public void InboundAddStoryCardCmdNotPropagatedAgain()
        {
            var channel = new Mock<IMessageChannel>();
            var ea = new Mock<IEventAggregator>();

            Guid id = Guid.NewGuid();

            var cmd = new AddStoryCard(id, 1.0, 2.0, 3.0);
            var @event = new StoryCardAdded(id, 1.0, 2.0, 3.0);

            var hub = new MessageHub(channel.Object, ea.Object);

            ea.Setup(e => e.Publish(It.IsAny<AddStoryCard>())).Callback(() => hub.Handle(@event));

            hub.Handle(cmd);

            channel.Verify(c => c.Send(@event), Times.Never());
            ea.Verify(e => e.Publish(cmd), Times.Once());
        }

        [TestMethod]
        public void InboundMoveCaretCmdNotPropagatedAgain()
        {
            var channel = new Mock<IMessageChannel>();
            var ea = new Mock<IEventAggregator>();

            Guid id = Guid.NewGuid();

            var cmd = new MoveCaret(id, "1", 123);
            var @event = new CaretMoved(id, "1", 123);

            var hub = new MessageHub(channel.Object, ea.Object);

            ea.Setup(e => e.Publish(It.IsAny<MoveCaret>())).Callback(() => hub.Handle(@event));

            hub.Handle(cmd);

            channel.Verify(c => c.Send(@event), Times.Never());
            ea.Verify(e => e.Publish(cmd), Times.Once());
        }
    }
}