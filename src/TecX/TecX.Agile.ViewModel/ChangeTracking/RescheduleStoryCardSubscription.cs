using System;
using System.Linq;

using TecX.Agile.ViewModel.Messages;
using TecX.Agile.ViewModel.Undo;
using TecX.Common;
using TecX.Common.Event;
using TecX.Undo;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class RescheduleStoryCardSubscription : IChangeSubscription
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly StoryCardCollection _collection;
        private IDisposable _subscription;

        public RescheduleStoryCardSubscription(IEventAggregator eventAggregator, StoryCardCollection collection)
        {
            Guard.AssertNotNull(collection, "collection");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
            _collection = collection;

            var rescheduled =
                from evt in Observable.FromEvent<StoryCardRescheduledEventArgs>(_collection, "StoryCardRescheduled")
                select new { evt.EventArgs.StoryCard, evt.EventArgs.From, evt.EventArgs.To};

            _subscription = rescheduled.Subscribe(x =>
                                                      {
                                                          StoryCardRescheduled rescheduledStoryCard =
                                                              new StoryCardRescheduled(x.StoryCard, x.From, x.To);

                                                          _eventAggregator.Publish(rescheduledStoryCard);

                                                          //TODO weberse this decision must be moved to the appropriate handler!!

                                                          //var currentAction =
                                                          //    _actionManager.CurrentAction as RescheduleStoryCardAction;

                                                          //if(currentAction != null)
                                                          //{
                                                          //    if(currentAction.StoryCard == evt.EventArgs.StoryCard &&
                                                          //        currentAction.To == evt.EventArgs.To &&
                                                          //        currentAction.From == evt.EventArgs.From)
                                                          //    {
                                                          //        //we are in a redo situation -> dont issue another action
                                                          //        return;
                                                          //    }

                                                          //    if(currentAction.StoryCard == evt.EventArgs.StoryCard &&
                                                          //        currentAction.To == evt.EventArgs.From &&
                                                          //        currentAction.From == evt.EventArgs.To)
                                                          //    {
                                                          //        //undo -> do nothing
                                                          //        return;
                                                          //    }
                                                          //}

                                                          //RescheduleStoryCardAction action =
                                                          //    new RescheduleStoryCardAction(evt.EventArgs.StoryCard,
                                                          //                                  evt.EventArgs.From,
                                                          //                                  evt.EventArgs.To);

                                                          //_actionManager.RecordAction(action);
                                                      });

        }

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
                _subscription = null;
            }
        }
    }
}