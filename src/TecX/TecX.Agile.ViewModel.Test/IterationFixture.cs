using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Services;
using TecX.Common.Event;
using TecX.TestTools;
using TecX.Undo;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class IterationFixture
    {
        [TestMethod]
        public void WhenPostponingStoryCard_EventIsRaised()
        {
            var mock = new Mock<IEventAggregator>();

            StoryCard card = new StoryCard
                                 {
                                     Id = Guid.NewGuid()
                                 };

            Iteration iteration = new Iteration
                                      {
                                          Id = Guid.NewGuid(),
                                          EventAggregator = mock.Object
                                      };

            iteration.Add(card);

            Project project = new Project
                                  {
                                      iteration
                                  };

            iteration.Postpone(card);

            mock.Verify(ea => ea.Publish(It.Is<StoryCardPostponed>(msg => card.Id == msg.StoryCardId && iteration.Id == msg.From)));
        }

        [TestMethod]
        public void WhenPostponingStoryCard_NewParentIsSet()
        {
            Iteration iteration = new Iteration();

            StoryCard card = new StoryCard();

            iteration.Add(card);

            Project project = new Project();

            project.Add(iteration);

            iteration.Postpone(card);

            Assert.AreEqual(project.Backlog, card.Parent);
        }

        [TestMethod]
        public void WhenPostponingStoryCard_IsRemovedFromIteration()
        {
            Iteration iteration = new Iteration();

            StoryCard card = new StoryCard();

            iteration.Add(card);

            Project project = new Project();

            project.Add(iteration);

            iteration.Postpone(card);

            Assert.IsFalse(iteration.Contains(card));
        }

        [TestMethod]
        public void WhenPostponingStoryCard_IsAddedToBacklog()
        {
            Iteration iteration = new Iteration();

            StoryCard card = new StoryCard();

            iteration.Add(card);

            Project project = new Project();

            project.Add(iteration);

            iteration.Postpone(card);

            Assert.IsTrue(project.Backlog.Contains(card));
        }
    }
}
