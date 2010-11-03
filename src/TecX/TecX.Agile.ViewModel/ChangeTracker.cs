using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using TecX.Common;
using TecX.Undo;
using TecX.Undo.Actions;

namespace TecX.Agile.ViewModel
{
    public class ChangeTracker : IChangeTracker
    {
        private class ChangeTrackingSubscription : IDisposable
        {
            private readonly IDisposable _subscription;
            private readonly IActionManager _actionManager;

            public ChangeTrackingSubscription(IActionManager actionManager, PlanningArtefact item)
            {
                Guard.AssertNotNull(actionManager, "actionManager");
                Guard.AssertNotNull(item, "item");

                _actionManager = actionManager;

                var changing = from evt in Observable.FromEvent<PropertyChangingEventArgs>(item, "PropertyChanging")
                               select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

                var changed = from evt in Observable.FromEvent<PropertyChangedEventArgs>(item, "PropertyChanged")
                              select new { evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

                var beforeAfter = changing
                    .CombineLatest(changed, (before, after) => new { ParentObject = before.Sender, before.PropertyName, OldValue = before.Value, NewValue = after.Value })
                    .Where(ba => ba.NewValue != ba.OldValue);

                _subscription = beforeAfter.Subscribe(x =>
                                                          {
                                                              var action = new SetPropertyAction(x.ParentObject,
                                                                                                 x.PropertyName,
                                                                                                 x.OldValue, 
                                                                                                 x.NewValue);

                                                              _actionManager.RecordAction(action);
                                                          });
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
                Dispose(true); 
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if(disposing)
                {
                    _subscription.Dispose();
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
            _subscriptions = new Dictionary<Guid, ChangeTrackingSubscription>();
        }

        #endregion c'tor

        #region Implementation of IChangeTracker

        public void Subscribe(PlanningArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            ChangeTrackingSubscription subscription = new ChangeTrackingSubscription(_actionManager, item);

            _subscriptions.Add(item.Id, subscription);
        }

        public void Unsubscribe(PlanningArtefact item)
        {
            ChangeTrackingSubscription existing;
            if(_subscriptions.TryGetValue(item.Id, out existing))
            {
                _subscriptions.Remove(item.Id);
                existing.Dispose();
            }
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
}