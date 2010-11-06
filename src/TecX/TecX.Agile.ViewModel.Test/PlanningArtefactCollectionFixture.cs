using System;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class PlanningArtefactCollectionFixture
    {
        [TestMethod]
        public void WhenAddingStoryCard_IsAdded()
        {
            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            Assert.IsTrue(collection.Contains(card));
        }

        [TestMethod]
        public void WhenRemovingStoryCard_IsRemoved()
        {
            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            collection.Remove(card.Id);

            Assert.IsFalse(collection.Contains(card));
        }

        [TestMethod]
        public void WhenReplacingStoryCard_IsReplaced()
        {
            StoryCardCollection collection = new StoryCardCollection();

            Guid id = Guid.NewGuid();

            StoryCard card = new StoryCard { Id = id, Name = "old card" };

            StoryCard replacement = new StoryCard { Id = id, Name = "new card" };

            collection.Add(card);

            collection[id] = replacement;

            Assert.IsTrue(collection.Contains(replacement));
            Assert.IsFalse(collection.Contains(card));
        }

        [TestMethod]
        public void WhenReplacingStoryCard_NotificationIsRaised()
        {
            StoryCardCollection collection = new StoryCardCollection();

            Guid id = Guid.NewGuid();

            StoryCard card = new StoryCard { Id = id };

            StoryCard replacement = new StoryCard { Id = id };

            collection.Add(card);

            bool notified = false;

            collection.CollectionChanged += (s, e) =>
                                                {
                                                    Assert.AreEqual(s, collection);
                                                    Assert.AreEqual(NotifyCollectionChangedAction.Replace, e.Action);
                                                    Assert.AreEqual(card, e.OldItems[0]);
                                                    Assert.AreEqual(replacement, e.NewItems[0]);
                                                    notified = true;
                                                };

            collection[id] = replacement;

            Assert.IsTrue(notified, "replacing storycard must trigger notification");
        }

        [TestMethod]
        public void WhenAddingStoryCard_NotificationIsRaised()
        {
            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            bool notified = false;

            collection.CollectionChanged += (s, e) =>
                                                {
                                                    Assert.AreEqual(collection, s);
                                                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                                                    Assert.AreEqual(card, e.NewItems[0]);
                                                    notified = true;
                                                };

            collection.Add(card);

            Assert.IsTrue(notified, "adding storycard must trigger event");
        }

        [TestMethod]
        public void WhenRemovingStoryCard_NotificationIsRaised()
        {
            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            bool notified = false;

            collection.CollectionChanged += (s, e) =>
            {
                Assert.AreEqual(collection, s);
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(card, e.OldItems[0]);
                notified = true;
            };

            collection.Remove(card.Id);

            Assert.IsTrue(notified, "adding storycard must trigger event");
        }

        [TestMethod]
        public void WhenAddingStoryCard_ParentIsSet()
        {
            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            Assert.AreEqual(collection, card.Parent);
        }

        [TestMethod]
        public void WhenRemovingStoryCard_ParentIsRemoved()
        {
            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            collection.Remove(card.Id);

            Assert.IsNull(card.Parent);
        }
    }
}
