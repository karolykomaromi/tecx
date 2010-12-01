using System;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.ViewModel.Undo;
using TecX.Common.Event;
using TecX.Undo;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class ChangeTrackerFixture
    {
        [TestMethod]
        public void WhenChangingPropertyOnSubscribedStoryCard_TrackerIssuesPropertyChangedMessage()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            Guid id = Guid.NewGuid();
            Guid newId = Guid.NewGuid();

            StoryCard card = new StoryCard { Id = id };

            tracker.Subscribe(card);

            card.Id = newId;

            mockEventAggregator
                .Verify(ea => ea.Publish(
                    It.Is<PropertyUpdated>(pc => card.Id == pc.ArtefactId &&
                                                 "Id" == pc.PropertyName &&
                                                 id == (Guid)pc.OldValue &&
                                                 newId == (Guid)pc.NewValue)));

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenSubscribingSameItemMoreThanOnce_Throws()
        {
            var mockActionManager = new Mock<IActionManager>();
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            StoryCard card = new StoryCard();

            tracker.Subscribe(card);

            tracker.Subscribe(card);
        }

        [TestMethod]
        public void WhenAddingItemToSubscribedPlanningArtefactCollection_TrackerIssuesCollectionChangedMessage()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            PlanningArtefactCollection<StoryCard> collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            tracker.Subscribe(collection);

            collection.Add(card);

            mockEventAggregator
                .Verify(ea => ea.Publish(
                    It.Is<StoryCardAdded>(
                        msg => msg.To == collection && msg.StoryCard == card)),
                    Times.Once(),
                    "adding item must trigger CollectionChanged message");

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        public void WhenRemovingItemFromSubscribedPlanningArtefactCollection_TrackerIssuesCollectionChangedMessage()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            PlanningArtefactCollection<StoryCard> collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            tracker.Subscribe(collection);

            collection.Remove(card.Id);

            mockEventAggregator
                .Verify(ea => ea.Publish(
                    It.Is<StoryCardRemoved>(
                        msg => msg.From == collection && msg.StoryCard == card)),
                    Times.Once(),
                    "removing item must trigger CollectionChanged message");

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        public void WhenUnsubscribingItem_TrackerDoesNotIssueAnyMoreMessages()
        {
            var mockActionManager = new Mock<IActionManager>();
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            string name = "old name";
            string newName = "newName";

            StoryCard card = new StoryCard();

            tracker.Subscribe(card);


            card.Name = name;

            tracker.Unsubscribe(card);

            card.Name = newName;

            mockEventAggregator
                .Verify(ea => ea.Publish(It.IsAny<PropertyUpdated>()), Times.Once());

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        public void WhenReschedulingItemInStoryCardCollection_TrackerIssuesRescheduledMessage()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            Iteration iter1 = new Iteration { Id = Guid.NewGuid() };
            Iteration iter2 = new Iteration { Id = Guid.NewGuid() };
            StoryCard card = new StoryCard { Id = Guid.NewGuid() };

            iter1.Add(card);

            tracker.Subscribe((StoryCardCollection)iter1);
            tracker.Subscribe((StoryCardCollection)iter2);

            iter1.Reschedule(card, iter2);

            mockEventAggregator
                .Verify(ea => ea.Publish(It.Is<StoryCardRescheduled>(msg => msg.StoryCard == card &&
                                                                            msg.From == iter1 &&
                                                                            msg.To == iter2)),
                        Times.Once());

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        public void WhenUnsubscribingCollection_TrackeDoesNotIssueAnyMoreMessages()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            PlanningArtefactCollection<StoryCard> collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            tracker.Subscribe(collection);

            collection.Add(card);

            tracker.Unsubscribe(collection);

            collection.Remove(card.Id);

            mockEventAggregator
                .Verify(
                    ea => ea.Publish(It.IsAny<StoryCardAdded>()),
                    Times.Once());

            mockEventAggregator
                .Verify(
                    ea => ea.Publish(It.IsAny<StoryCardRemoved>()),
                    Times.Never());

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        public void WhenSubscribingIteration_TrackerIssuesPostponedMessage()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            Project project = new Project();
            Iteration iteration = new Iteration();
            StoryCard card = new StoryCard();

            iteration.Add(card);
            project.Add(iteration);

            tracker.Subscribe(iteration);

            iteration.Postpone(card);

            mockEventAggregator
                .Verify(
                    ea => ea.Publish(It.Is<StoryCardPostponed>(msg => msg.StoryCard == card && msg.From == iteration)),
                    Times.Once());

            mockEventAggregator.VerifyAll();
        }

        [TestMethod]
        public void WhenUnsubscribingIteraton_TrackerIssuesNoMoreMessages()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            Project project = new Project();
            Iteration iteration = new Iteration();
            StoryCard card = new StoryCard();
            StoryCard card2 = new StoryCard { Id = Guid.NewGuid() };

            iteration.Add(card);
            iteration.Add(card2);
            project.Add(iteration);

            tracker.Subscribe(iteration);

            iteration.Postpone(card);

            tracker.Unsubscribe(iteration);

            iteration.Postpone(card2);

            mockEventAggregator
                .Verify(
                    ea => ea.Publish(It.Is<StoryCardPostponed>(msg => msg.StoryCard == card && msg.From == iteration)),
                    Times.Once());

            mockEventAggregator.VerifyAll();
        }


        [TestMethod]
        [Ignore]
        public void WhenExecutingIssuedRescheduleStoryCardAction_DoesNotIssueTwice()
        {
            //TODO weberse must be migrated when a handler for change message is implemented

            var mockActionManager = new Mock<IActionManager>();
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);



            mockActionManager
                .Setup(am => am.RecordAction(It.IsAny<RescheduleStoryCardAction>()))
                .Callback((IAction action) => action.Execute());

            Iteration iter1 = new Iteration { Id = Guid.NewGuid() };
            Iteration iter2 = new Iteration { Id = Guid.NewGuid() };
            StoryCard card = new StoryCard { Id = Guid.NewGuid() };

            iter1.Add(card);

            tracker.Subscribe((StoryCardCollection)iter1);
            tracker.Subscribe((StoryCardCollection)iter2);

            iter1.Reschedule(card, iter2);

            mockActionManager
                .Verify(am =>
                    am.RecordAction(
                        It.IsAny<RescheduleStoryCardAction>()),
                    Times.Once());

            mockActionManager.VerifyAll();
        }

        [TestMethod]
        [Ignore]
        public void WhenUndoingAdd_TrackerDoesNotIssueAnotherAction()
        {
            //TODO weberse must be migrated when a handler for change message is implemented

            var mockActionManager = new Mock<IActionManager>();
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            CollectionChangedAction<StoryCard> undoableAction = null;

            mockActionManager.SetupGet(ma => ma.CurrentAction).Returns(() => undoableAction);

            mockActionManager.Setup(
                ma =>
                ma.RecordAction(
                    It.Is<CollectionChangedAction<StoryCard>>(
                        action => action.Action == NotifyCollectionChangedAction.Add)))
                .Callback((IAction action) =>
                {
                    undoableAction = (CollectionChangedAction<StoryCard>)action;
                    undoableAction.Execute();
                });

            mockActionManager.Setup(
                ma =>
                ma.RecordAction(
                    It.Is<CollectionChangedAction<StoryCard>>(
                        action => action.Action == NotifyCollectionChangedAction.Remove))).Callback(
                            () => Assert.Fail("undoing action must not issue another action"));

            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            tracker.Subscribe(collection);

            collection.Add(card);

            Assert.IsNotNull(undoableAction);

            undoableAction.UnExecute();

            Assert.AreEqual(0, collection.Count());
        }

        [TestMethod]
        [Ignore]
        public void WhenUndoingReschedule_TrackerDoesNotIssueAnotherAction()
        {
            //TODO weberse must be migrated when a handler for change message is implemented

            var mockActionManager = new Mock<IActionManager>();
            var mockEventAggregator = new Mock<IEventAggregator>();

            IChangeTracker tracker = new ChangeTracker(mockEventAggregator.Object);

            RescheduleStoryCardAction currentAction = null;

            mockActionManager.SetupGet(am => am.CurrentAction).Returns(() => currentAction);

            mockActionManager
                .Setup(am => am.RecordAction(It.IsAny<RescheduleStoryCardAction>()))
                .Callback((IAction a) =>
                {
                    currentAction = (RescheduleStoryCardAction)a;
                    a.Execute();
                });

            Iteration iter1 = new Iteration { Id = Guid.NewGuid() };
            Iteration iter2 = new Iteration { Id = Guid.NewGuid() };
            StoryCard card = new StoryCard { Id = Guid.NewGuid() };

            iter1.Add(card);

            tracker.Subscribe((StoryCardCollection)iter1);
            tracker.Subscribe((StoryCardCollection)iter2);

            iter1.Reschedule(card, iter2);

            currentAction.UnExecute();

            mockActionManager.Verify(am => am.RecordAction(It.IsAny<RescheduleStoryCardAction>()), Times.Once());

            mockActionManager.VerifyAll();
        }
    }
}
