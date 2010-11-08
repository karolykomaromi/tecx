using System;
using System.Linq;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class RescheduleStoryCardSubscription : IChangeSubscription
    {
        private readonly IActionManager _actionManager;
        private readonly StoryCardCollection _collection;
        private IDisposable _subscription;

        public RescheduleStoryCardSubscription(IActionManager actionManager, StoryCardCollection collection)
        {
            Guard.AssertNotNull(actionManager, "actionManager");
            Guard.AssertNotNull(collection, "collection");

            _actionManager = actionManager;
            _collection = collection;

            var rescheduled =
                from evt in Observable.FromEvent<StoryCardRescheduledEventArgs>(_collection, "StoryCardRescheduled")
                select evt;

            _subscription = rescheduled.Subscribe(evt =>
                                                      {
                                                          var currentAction =
                                                              _actionManager.CurrentAction as RescheduleStoryCardAction;

                                                          if(currentAction != null)
                                                          {
                                                              if(currentAction.StoryCard == evt.EventArgs.StoryCard &&
                                                                  currentAction.To == evt.EventArgs.To &&
                                                                  currentAction.From == evt.EventArgs.From)
                                                              {
                                                                  //we are in a redo situation -> dont issue another action
                                                                  return;
                                                              }

                                                              if(currentAction.StoryCard == evt.EventArgs.StoryCard &&
                                                                  currentAction.To == evt.EventArgs.From &&
                                                                  currentAction.From == evt.EventArgs.To)
                                                              {
                                                                  //undo -> do nothing
                                                                  return;
                                                              }
                                                          }

                                                          RescheduleStoryCardAction action =
                                                              new RescheduleStoryCardAction(evt.EventArgs.StoryCard,
                                                                                            evt.EventArgs.From,
                                                                                            evt.EventArgs.To);

                                                          _actionManager.RecordAction(action);
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