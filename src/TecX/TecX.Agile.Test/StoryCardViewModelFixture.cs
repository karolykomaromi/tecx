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

            var vm = new StoryCardViewModel(ea.Object) { Id = id };

            var cmd = new ChangeProperty(id, "TaskOwner", string.Empty, "John Wayne");

            vm.Handle(cmd);

            ea.Verify(e => e.Publish(
                It.Is((PropertyChanged pc) =>
                    pc.ArtefactId == id &&
                    pc.PropertyName == "TaskOwner" &&
                    pc.Before == null &&
                    Equals(pc.After, "John Wayne"))),
                Times.Once());

            Assert.AreEqual("John Wayne", vm.TaskOwner);
        }
    }
}
