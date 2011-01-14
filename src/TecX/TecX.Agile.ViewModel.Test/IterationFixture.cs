using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Agile.Infrastructure.Services;
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
            

            Iteration iteration = new Iteration();

            StoryCard card = new StoryCard();

            iteration.Add(card);

            Project project = new Project();

            project.Add(iteration);

            iteration
                .AfterCalling(i => i.Postpone(card))
                .ShouldNotifyVia<StoryCardPostponedEventArgs>("StoryCardPostponed")
                .WithArgs(args =>
                              {
                                  Assert.AreEqual(card, args.StoryCard);
                                  Assert.AreEqual(iteration, args.From);
                              })
                .AssertExpectations();
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
