using System;
using System.ComponentModel;
using System.Linq;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.ViewModel;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ChangeTracking
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
                beforeAfter.Subscribe(x =>
                                          {
                                              //if we move a storycard we dont want a simple property change notification
                                              if (x.PropertyName == Infrastructure.Constants.PropertyNames.X ||
                                                  x.PropertyName == Infrastructure.Constants.PropertyNames.Y ||
                                                  x.PropertyName == Infrastructure.Constants.PropertyNames.Angle)
                                              {
                                                  StoryCard card = x.ParentObject as StoryCard;

                                                  if (card != null)
                                                  {
                                                      //but a more specific event that tells us that a card was moved
                                                      var storyCardMoved = new StoryCardMoved(card, card.X, card.Y, card.Angle);

                                                      _eventAggregator.Publish(storyCardMoved);
                                                      return;
                                                  }
                                              }

                                              _eventAggregator.Publish(
                                                  new PropertyChanged(
                                                      x.ParentObject,
                                                      x.PropertyName,
                                                      x.OldValue,
                                                      x.NewValue));
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
            if (disposing)
            {
                _subscription.Dispose();
            }
        }

        #endregion
    }
}