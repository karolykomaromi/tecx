using System;
using System.ComponentModel;
using System.Linq;

using TecX.Agile.ViewModel.Messages;
using TecX.Common;
using TecX.Common.Event;
using TecX.Undo;
using TecX.Undo.Actions;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    internal class PropertyChangeSubscription : IChangeSubscription
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDisposable _subscription;

        public PropertyChangeSubscription(PlanningArtefact item, IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(item, "item");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;

            var changing = from evt in Observable.FromEvent<PropertyChangingEventArgs>(item, "PropertyChanging")
                           select new { Sender = (PlanningArtefact)evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

            var changed = from evt in Observable.FromEvent<PropertyChangedEventArgs>(item, "PropertyChanged")
                          select new { Sender = (PlanningArtefact)evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

            var beforeAfter = changing
                .CombineLatest(changed, (before, after) => new
                                                               {
                                                                   ParentObject = before.Sender,
                                                                   before.PropertyName,
                                                                   OldValue = before.Value,
                                                                   NewValue = after.Value
                                                               })
                .Where(ba => ba.NewValue != ba.OldValue);

            _subscription =
                beforeAfter.Subscribe(x => _eventAggregator.Publish(
                                                    new PropertyChanged(
                                                        x.ParentObject,
                                                        x.PropertyName,
                                                        x.OldValue,
                                                        x.NewValue)));
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription.Dispose();
            }
        }

        #endregion
    }
}