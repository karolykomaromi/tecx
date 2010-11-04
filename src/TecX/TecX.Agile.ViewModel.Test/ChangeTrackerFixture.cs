using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Undo;
using TecX.Undo.Actions;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class ChangeTrackerFixture
    {
        [TestMethod]
        public void WhenSubscribingStoryCard_TrackerIssuesAction()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            Guid id = Guid.NewGuid();
            Guid newId = Guid.NewGuid();

            StoryCard card = new StoryCard { Id = id };

            tracker.Subscribe(card);

            card.Id = newId;

            mockActionManager
                .Verify(am => am.RecordAction(
                    It.Is<SetPropertyAction>(action =>
                                             (Guid)action.OldValue == id &&
                                             (Guid)action.NewValue == newId &&
                                             action.ParentObject == card &&
                                             action.Property.Name == "Id")),
                    Times.Once(),
                    "changing Id must trigger an action");

            mockActionManager.VerifyAll();
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
        public void WhenSubscribingStoryCardCollection_TrackerIssuesActionOnAdd()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            StoryCardCollection collection = new StoryCardCollection();

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
        public void WhenSubscribingStoryCardCollection_TrackerIssuesActionOnRemove()
        {
            var mockActionManager = new Mock<IActionManager>();

            IChangeTracker tracker = new ChangeTracker(mockActionManager.Object);

            StoryCardCollection collection = new StoryCardCollection();

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
                                  undoableAction = (CollectionChangedAction<StoryCard>) action;
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
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Customer customer = new Customer { Name = "old name" };

    //        var changing = from evt in Observable.FromEvent<PropertyChangingEventArgs>(customer, "PropertyChanging")
    //                       select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

    //        var changed = from evt in Observable.FromEvent<PropertyChangedEventArgs>(customer, "PropertyChanged")
    //                      select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

    //        var beforeAfter = changing
    //                            .CombineLatest(changed, (before, after) => new { Before = before.Value, After = after.Value })
    //                            .Where(ba => ba.After != ba.Before);

    //        beforeAfter.Subscribe(x => Console.WriteLine(x.Before + " " + x.After));

    //        for (int i = 0; i < 1000; i++)
    //        {
    //            customer.Name = i.ToString();
    //        }
    //    }
    //}

    //public class Customer : INotifyPropertyChanging, INotifyPropertyChanged
    //{
    //    private string _name;

    //    public string Name
    //    {
    //        get { return _name; }
    //        set
    //        {
    //            if (value == _name)
    //                return;

    //            OnPropertyChanging(() => Name);
    //            _name = value;
    //            OnPropertyChanged(() => Name);
    //        }
    //    }

    //    public event PropertyChangingEventHandler PropertyChanging;
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    private void OnPropertyChanging<T>(Expression<Func<T>> expression)
    //    {
    //        MemberExpression memberSelector = (MemberExpression)expression.Body;

    //        if (PropertyChanging != null)
    //        {
    //            PropertyChanging(this, new PropertyChangingEventArgs(memberSelector.Member.Name));
    //        }
    //    }

    //    private void OnPropertyChanged<T>(Expression<Func<T>> expression)
    //    {
    //        MemberExpression memberSelector = (MemberExpression)expression.Body;

    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(this, new PropertyChangedEventArgs(memberSelector.Member.Name));
    //        }
    //    }
    //}
}
