namespace TecX.Agile.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.ViewModels;
    using TecX.Event;

    [TestClass]
    public class StoryCardViewModelFixture
    {
        [TestMethod]
        public void CanUpdatePropertyUsingMessage()
        {
            Guid id = Guid.NewGuid();

            var ea = new Mock<IEventAggregator>();

            var vm = StoryCardViewModel.Create(ea.Object, id, 0.0, 0.0, 0.0);

            var cmd = new ChangeProperty(id, "TaskOwner", string.Empty, "John Wayne");

            vm.Handle(cmd);

            ea.Verify(e => e.Publish(
                It.Is((PropertyChanged pc) =>
                    pc.Id == id &&
                    pc.PropertyName == "TaskOwner" &&
                    Equals(pc.From, string.Empty) &&
                    Equals(pc.To, "John Wayne"))),
                Times.Once());

            Assert.AreEqual("John Wayne", vm.TaskOwner);
        }
    }
}
