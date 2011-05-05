using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Agile.Infrastructure.Events;
using TecX.Common.Event;
using TecX.TestTools;

namespace TecX.Agile.ViewModel.Test
{
    public abstract class Given_TwoStoryCardCollections : GivenWhenThen
    {
        protected Iteration _from;
        protected Iteration _to;
        protected StoryCard _card;
        protected Mock<IEventAggregator> _mock;

        protected override void Given()
        {
            _mock = new Mock<IEventAggregator>();

            _from = new Iteration { Id = Guid.NewGuid(), EventAggregator = _mock.Object };

            _to = new Iteration { Id = Guid.NewGuid() };

            _card = new StoryCard { Id = Guid.NewGuid() };

            _from.Add(_card);
        }
    }

    [TestClass]
    public class When_ReschedulingStoryCard : Given_TwoStoryCardCollections
    {
        protected override void When()
        {
            _from.Reschedule(_card, _to);
        }

        [TestMethod]
        public void Then_NotificationIsPublishedViaEventAggregator()
        {
            _mock.Verify(ea => ea.Publish(It.Is<StoryCardRescheduled>(msg => _card.Id == msg.StoryCardId && _from.Id == msg.From && _to.Id == msg.To)), 
                Times.Once(), 
                "ea not called to publish message");
        }

        [TestMethod]
        public void Then_NewStoryCardParentIsSet()
        {
            Assert.AreEqual(_to, _card.Parent);
        }

        [TestMethod]
        public void Then_CardIsRemovedFromOldParent()
        {
            Assert.IsFalse(_from.Contains(_card));
        }

        [TestMethod]
        public void Then_CardIsAddedToNewParent()
        {
            Assert.IsTrue(_to.Contains(_card));
        }
    }
}
