using System;
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TecX.Undo;
using TecX.Common;
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

            mockActionManager
                .Verify(am => am.RecordAction(
                    It.Is<SetPropertyAction>(action =>
                                             (Guid) action.OldValue == id &&
                                             (Guid) action.Value == newId &&
                                             action.ParentObject == card &&
                                             action.Property.Name == "Id")), 
                    Times.Once(),
                    "changing Id must trigger an action");

            tracker.Subscribe(card);

            card.Id = newId;

            mockActionManager.VerifyAll();
        }
    }

    public class ChangeTracker : IChangeTracker
    {
        private class ChangeTrackingSubscription : IDisposable
        {
            public ChangeTrackingSubscription(PlanningArtefact item)
            {
                Guard.AssertNotNull(item, "item");
                var changing = from evt in Observable.FromEvent<PropertyChangingEventArgs>(item, "PropertyChanging")
                               select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

                var changed = from evt in Observable.FromEvent<PropertyChangedEventArgs>(item, "PropertyChanged")
                              select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

                var beforeAfter = changing
                                    .CombineLatest(changed, (before, after) => new { Before = before.Value, After = after.Value })
                                    .Where(ba => ba.After != ba.Before);

                beforeAfter.Subscribe(x => Console.WriteLine(x.Before + " " + x.After));
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool disposing)
            {
                if(disposing)
                {
                    
                }
            }

            #endregion
        }

        #region Fields

        private readonly IActionManager _actionManager;
        private readonly Dictionary<Guid, ChangeTrackingSubscription> _subscriptions;

        #endregion Fields

        #region c'tor

        public ChangeTracker(IActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actionManager = actionManager;
        }

        #endregion c'tor

        #region Implementation of IChangeTracker

        public void Subscribe(PlanningArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            ChangeTrackingSubscription subscription = new ChangeTrackingSubscription(item);

            _subscriptions.Add(item.Id, subscription);

        }

        public void Unsubscribe(PlanningArtefact item)
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection) where TArtefact : PlanningArtefact
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection) where TArtefact : PlanningArtefact
        {
            throw new NotImplementedException();
        }

        #endregion
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
