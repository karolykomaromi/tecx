using System;
using System.ComponentModel;
using System.Linq;

using TecX.Common;
using TecX.Undo;
using TecX.Undo.Actions;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    internal class PropertyChangeSubscription : IChangeSubscription
    {
        private readonly IDisposable _subscription;
        private readonly IActionManager _actionManager;

        public PropertyChangeSubscription(IActionManager actionManager, PlanningArtefact item)
        {
            Guard.AssertNotNull(actionManager, "actionManager");
            Guard.AssertNotNull(item, "item");

            _actionManager = actionManager;

            var changing = from evt in Observable.FromEvent<PropertyChangingEventArgs>(item, "PropertyChanging")
                           select new { Sender = (PlanningArtefact)evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

            var changed = from evt in Observable.FromEvent<PropertyChangedEventArgs>(item, "PropertyChanged")
                          select new { Sender = (PlanningArtefact)evt.Sender, evt.EventArgs.PropertyName, Value = evt.Sender.GetType().GetProperty(evt.EventArgs.PropertyName).GetValue(evt.Sender, null) };

            var beforeAfter = changing
                .CombineLatest(changed, (before, after) => new { ParentObject = before.Sender, before.PropertyName, OldValue = before.Value, NewValue = after.Value })
                .Where(ba => ba.NewValue != ba.OldValue);


            //TODO weberse instead of hard wiring a single handler we can use some kind of ChangeHandlerChain that contains
            //different handlers (e.g. for P2P distribution of changed properties, tracing information or the like)
            var handlerChain = new PropertyChangeHandlerChain();
            handlerChain.Add((parentObject, propertyName, oldValue, newValue) =>
                                 {
                                     var action = new SetPropertyAction(parentObject,
                                                                        propertyName,
                                                                        oldValue,
                                                                        newValue);

                                     _actionManager.RecordAction(action);
                                 });

            _subscription = beforeAfter.Subscribe(x => handlerChain
                .Handle(x.ParentObject, x.PropertyName, x.OldValue, x.NewValue));
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
}