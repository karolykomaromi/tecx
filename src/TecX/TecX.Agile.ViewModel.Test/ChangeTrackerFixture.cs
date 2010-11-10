using System;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Agile.ViewModel.ChangeTracking;
using TecX.Agile.ViewModel.Undo;
using TecX.Undo;
using TecX.Undo.Actions;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class ChangeTrackerFixture
    {
        [TestMethod]
        public void WhenChangingPropertyOnSubscribedStoryCard_EventSourceRaisesPropertyChangedEvent()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            Guid id = Guid.NewGuid();
            Guid newId = Guid.NewGuid();

            StoryCard card = new StoryCard { Id = id };

            tracker.Subscribe(card);

            bool notified = false;

            EventHandler<ArtefactPropertyChangedEventArgs> handler = (s, e) =>
                                                                         {
                                                                             Assert.AreEqual(card, e.ParentObject);
                                                                             Assert.AreEqual("Id", e.PropertyName);
                                                                             Assert.AreEqual<Guid>(id, (Guid)e.OldValue);
                                                                             Assert.AreEqual<Guid>(newId, (Guid)e.NewValue);
                                                                             notified = true;
                                                                         };

            EventSource.PropertyChanged += handler;

            card.Id = newId;

            Assert.IsTrue(notified);

            EventSource.PropertyChanged -= handler;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenSubscribingSameItemMoreThanOnce_Throws()
        {
            var mockActionManager = new Mock<IActionManager>();

            ChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            StoryCard card = new StoryCard();

            tracker.Subscribe(card);

            tracker.Subscribe(card);
        }

        [TestMethod]
        public void WhenAddingItemToSubscribedPlanningArtefactCollection_TrackerIssuesAction()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            PlanningArtefactCollection<StoryCard> collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            tracker.Subscribe(collection);

            collection.Add(card);

            mockActionManager
                .Verify(am => am.RecordAction(
                    It.Is<CollectionChangedAction<StoryCard>>(
                        action => action.Collection == collection && action.NewItems.First() == card)),
                    Times.Once(),
                    "adding item must trigger AddItemToCollectionAction");

            mockActionManager.VerifyAll();
        }

        [TestMethod]
        public void WhenRemovingItemFromSubscribedPlanningArtefactCollection_TrackerIssuesAction()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            PlanningArtefactCollection<StoryCard> collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            collection.Add(card);

            tracker.Subscribe(collection);

            collection.Remove(card.Id);

            mockActionManager
                .Verify(am => am.RecordAction(
                    It.Is<CollectionChangedAction<StoryCard>>(
                        action => action.Collection == collection && action.OldItems.First() == card)),
                    Times.Once(),
                    "adding item must trigger AddItemToCollectionAction");

            mockActionManager.VerifyAll();
        }

        [TestMethod]
        public void WhenUndoingAdd_TrackerDoesNotIssueAnotherAction()
        {
            var mockActionManager = new Mock<IActionManager>();

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

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            StoryCardCollection collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            tracker.Subscribe(collection);

            collection.Add(card);

            Assert.IsNotNull(undoableAction);

            undoableAction.UnExecute();

            Assert.AreEqual(0, collection.Count());
        }

        [TestMethod]
        public void WhenUnsubscribingItem_EventSourceDoesNotRaiseAnyMoreEvents()
        {
            var mockActionManager = new Mock<IActionManager>();

            string name = "old name";
            string newName = "newName";

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            StoryCard card = new StoryCard();

            tracker.Subscribe(card);

            int callCounter = 0;

            EventHandler<ArtefactPropertyChangedEventArgs> handler = (s, e) => callCounter++;

            EventSource.PropertyChanged += handler;

            card.Name = name;

            tracker.Unsubscribe(card);

            card.Name = newName;

            Assert.AreEqual(1, callCounter);

            EventSource.PropertyChanged -= handler;
        }

        [TestMethod]
        public void WhenUnsubscribingCollection_NoMoreActionsAreIssued()
        {
            var mockActionManager = new Mock<IActionManager>();

            PlanningArtefactCollection<StoryCard> collection = new StoryCardCollection();

            StoryCard card = new StoryCard();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            tracker.Subscribe(collection);

            collection.Add(card);

            tracker.Unsubscribe(collection);

            collection.Remove(card.Id);

            mockActionManager
                .Verify(
                    ma => ma.RecordAction(It.IsAny<CollectionChangedAction<StoryCard>>()),
                    Times.Once());

            mockActionManager.VerifyAll();
        }

        [TestMethod]
        public void WhenCallingIntoPropertyChangeSubscriptionHandlerChain_ExecutesAllHandlers()
        {
            PropertyChangeHandlerChain chain = new PropertyChangeHandlerChain();

            int handlersCalled = 0;

            chain.Add((parentObject, propertyName, oldValue, newValue) =>
                          {
                              handlersCalled++;
                          });

            chain.Add((parentObject, propertyName, oldValue, newValue) =>
            {
                handlersCalled++;
            });

            StoryCard card = new StoryCard();

            chain.Handle(card, "Name", null, "new name");

            Assert.AreEqual(2, handlersCalled);
        }

        [TestMethod]
        public void WhenReschedulingItemInStoryCardCollection_TrackerIssuesAction()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);
            
            Iteration iter1 = new Iteration { Id = Guid.NewGuid() };
            Iteration iter2 = new Iteration { Id = Guid.NewGuid() };
            StoryCard card = new StoryCard { Id = Guid.NewGuid() };

            iter1.Add(card);

            tracker.Subscribe(iter1);
            tracker.Subscribe(iter2);

            iter1.Reschedule(card, iter2);

            mockActionManager
                .Verify(am =>
                    am.RecordAction(
                        It.Is<RescheduleStoryCardAction>(action => action.StoryCard == card &&
                                                                   action.From == iter1 &&
                                                                   action.To == iter2)),
                    Times.Once());

            mockActionManager.VerifyAll();
        }

        [TestMethod]
        public void WhenExecutingIssuedRescheduleStoryCardAction_DoesNotIssueTwice()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            mockActionManager
                .Setup(am => am.RecordAction(It.IsAny<RescheduleStoryCardAction>()))
                .Callback((IAction action) => action.Execute());

            Iteration iter1 = new Iteration { Id = Guid.NewGuid() };
            Iteration iter2 = new Iteration { Id = Guid.NewGuid() };
            StoryCard card = new StoryCard { Id = Guid.NewGuid() };

            iter1.Add(card);

            tracker.Subscribe(iter1);
            tracker.Subscribe(iter2);

            iter1.Reschedule(card, iter2);

            mockActionManager
                .Verify(am =>
                    am.RecordAction(
                        It.IsAny<RescheduleStoryCardAction>()),
                    Times.Once());

            mockActionManager.VerifyAll();
        }

        [TestMethod]
        public void WhenUndoingReschedule_TrackerDoesNotIssueAnotherAction()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

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

            tracker.Subscribe(iter1);
            tracker.Subscribe(iter2);

            iter1.Reschedule(card, iter2);

            currentAction.UnExecute();

            mockActionManager.Verify(am => am.RecordAction(It.IsAny<RescheduleStoryCardAction>()), Times.Once());

            mockActionManager.VerifyAll();
        }
    }
}
