using System;
using System.Collections.Specialized;
using System.Linq;

using TecX.Agile.ViewModel.Messages;
using TecX.Agile.ViewModel.Undo;
using TecX.Common;
using TecX.Common.Event;
using TecX.Common.Extensions.Collections;
using TecX.Undo;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class CollectionChangeSubscription<TArtefact> : IChangeSubscription
        where TArtefact : PlanningArtefact
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly IDisposable _subscription;

        #endregion Fields

        #region c'tor

        public CollectionChangeSubscription(IEventAggregator eventAggregator, PlanningArtefactCollection<TArtefact> collection)
        {
            Guard.AssertNotNull(collection, "collection");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

           _eventAggregator = eventAggregator;

            var changed = from evt in Observable.FromEvent<NotifyCollectionChangedEventArgs>(collection, "CollectionChanged")
                          select new
                                     {
                                         Collection = (PlanningArtefactCollection<TArtefact>)evt.Sender,
                                         NewItems = evt.EventArgs.NewItems.AsEnumerable<TArtefact>(),
                                         OldItems = evt.EventArgs.OldItems.AsEnumerable<TArtefact>(),
                                         evt.EventArgs.Action
                                     };

            _subscription = changed.Subscribe(x =>
                                                  {
                                                      //TODO weberse this decision must be moved to a handler of the CollectionChanged<T> message!!

                                                      //var currentAction = _actionManager.CurrentAction as CollectionChangedAction<TArtefact>;
                                                      //if (currentAction != null)
                                                      //{
                                                      //    if (currentAction.Collection == x.Collection)
                                                      //    {
                                                      //        //TODO weberse what about replace?
                                                      //        if (((currentAction.Action == NotifyCollectionChangedAction.Add &&
                                                      //              x.Action == NotifyCollectionChangedAction.Remove) ||
                                                      //             (currentAction.Action == NotifyCollectionChangedAction.Remove &&
                                                      //              x.Action == NotifyCollectionChangedAction.Add)) &&
                                                      //            currentAction.OldItems.SequenceEqual(x.NewItems) &&
                                                      //            currentAction.NewItems.SequenceEqual(x.OldItems))
                                                      //        {
                                                      //            //undo -> do nothing
                                                      //            return;
                                                      //        }

                                                      //        if (currentAction.Action == x.Action &&
                                                      //            currentAction.NewItems.SequenceEqual(x.NewItems) &&
                                                      //            currentAction.OldItems.SequenceEqual(x.OldItems))
                                                      //        {
                                                      //            //redo -> do nothing
                                                      //            return;
                                                      //        }
                                                      //    }
                                                      //}

                                                      //var action = new CollectionChangedAction<TArtefact>(
                                                      //    x.Collection,
                                                      //    x.Action,
                                                      //    x.NewItems,
                                                      //    x.OldItems);

                                                      //_actionManager.RecordAction(action);

                                                      CollectionChanged<TArtefact> collectionChanged =
                                                          new CollectionChanged<TArtefact>(x.Collection, x.Action,
                                                                                           x.NewItems, x.OldItems);

                                                      _eventAggregator.Publish(collectionChanged);

                                                  });
        }

        #endregion c'tor

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

        #endregion Implementation of IDisposable
    }
}