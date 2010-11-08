using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.TestTools;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class StoryCardCollectionFixture
    {
        [TestMethod]
        public void WhenReschedulingStoryCard_EventIsRaised()
        {
            Iteration iter1 = new Iteration();
            Iteration iter2 = new Iteration();

            StoryCard card = new StoryCard();

            iter1.Add(card);

            iter1
                .AfterCalling(i => i.Reschedule(card, iter2))
                .ShouldNotifyVia<StoryCardRescheduledEventArgs>("StoryCardRescheduled")
                .WithArgs(args =>
                                {
                                    Assert.AreEqual(card, args.StoryCard);
                                    Assert.AreEqual(iter1, args.From);
                                    Assert.AreEqual(iter2, args.To);
                                })
                .AssertExpectations();
        }

        [TestMethod]
        public void WhenReschedulingStoryCard_NewStoryCardParentIsSet()
        {
            Iteration iter1 = new Iteration();
            Iteration iter2 = new Iteration();

            StoryCard card = new StoryCard();

            iter1.Add(card);

            iter1.Reschedule(card, iter2);

            Assert.AreEqual(iter2, card.Parent);
        }

        [TestMethod]
        public void WhenReschedulingStoryCard_CardIsRemovedFromOldParent()
        {
            Iteration iter1 = new Iteration();
            Iteration iter2 = new Iteration();

            StoryCard card = new StoryCard();

            iter1.Add(card);

            iter1.Reschedule(card, iter2);

            Assert.IsFalse(iter1.Contains(card));
        }

        [TestMethod]
        public void WhenReschedulingStoryCard_CardIsAddedToNewParent()
        {
            Iteration iter1 = new Iteration();
            Iteration iter2 = new Iteration();

            StoryCard card = new StoryCard();

            iter1.Add(card);

            iter1.Reschedule(card, iter2);

            Assert.IsTrue(iter2.Contains(card));
        }
    }
}
