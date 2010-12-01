using System;
using System.Collections.Specialized;
using System.Linq;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.ViewModel;
using TecX.Common;
using TecX.Common.Event;
using TecX.Common.Extensions.Collections;

namespace TecX.Agile.ChangeTracking
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

                                                      StoryCardCollection scc = x.Collection as StoryCardCollection;

                                                      if(scc != null)
                                                      {
                                                          switch(x.Action)
                                                          {
                                                              case NotifyCollectionChangedAction.Add:
                                                                  {
                                                                      StoryCard newItem =
                                                                          x.NewItems.First() as StoryCard;

                                                                      StoryCardAdded msg = new StoryCardAdded(newItem.Id, scc.Id);

                                                                      //can't use IMessage and generic publish because EA would get TypeParameter wrong
                                                                      _eventAggregator.Publish(msg);
                                                                  }
                                                                  return;
                                                              case NotifyCollectionChangedAction.Remove:
                                                                  {
                                                                      StoryCard oldItem =
                                                                          x.OldItems.First() as StoryCard;

                                                                      StoryCardRemoved msg = new StoryCardRemoved(oldItem.Id, scc.Id);

                                                                      //can't use IMessage and generic publish because EA would get TypeParameter wrong
                                                                      _eventAggregator.Publish(msg);
                                                                  }
                                                                  return;
                                                              case NotifyCollectionChangedAction.Replace:
                                                                  {

                                                                      StoryCard oldItem =
                                                                          x.OldItems.First() as StoryCard;
                                                                      
                                                                      StoryCard newItem =
                                                                          x.NewItems.First() as StoryCard;

                                                                      StoryCardReplaced msg = new StoryCardReplaced(oldItem.Id, newItem.Id, scc.Id);

                                                                      //can't use IMessage and generic publish because EA would get TypeParameter wrong
                                                                      _eventAggregator.Publish(msg);
                                                                  }
                                                                  return;
                                                              case NotifyCollectionChangedAction.Move:
                                                              case NotifyCollectionChangedAction.Reset:
                                                              default:
                                                                  throw new ArgumentOutOfRangeException();
                                                          }
                                                      }

                                                      IterationCollection ic = x.Collection as IterationCollection;

                                                      if(ic != null)
                                                      {
                                                          switch(x.Action)
                                                          {
                                                              case NotifyCollectionChangedAction.Add:
                                                                  {
                                                                      Iteration newItem =
                                                                          x.NewItems.First() as Iteration;

                                                                      IterationAdded msg = new IterationAdded(newItem.Id, ic.Id);

                                                                      //can't use IMessage and generic publish because EA would get TypeParameter wrong
                                                                      _eventAggregator.Publish(msg);
                                                                  }
                                                                  return;
                                                              case NotifyCollectionChangedAction.Remove:
                                                                  {
                                                                      Iteration oldItem =
                                                                          x.OldItems.First() as Iteration;

                                                                      IterationRemoved msg = new IterationRemoved(oldItem.Id, ic.Id);

                                                                      //can't use IMessage and generic publish because EA would get TypeParameter wrong
                                                                      _eventAggregator.Publish(msg);

                                                                  }
                                                                  return;
                                                              case NotifyCollectionChangedAction.Replace:
                                                                  {
                                                                      Iteration oldItem =
                                                                          x.OldItems.First() as Iteration;
                                                                      
                                                                      Iteration newItem =
                                                                          x.NewItems.First() as Iteration;

                                                                      IterationReplaced msg = new IterationReplaced(oldItem.Id, newItem.Id, ic.Id);

                                                                      //can't use IMessage and generic publish because EA would get TypeParameter wrong
                                                                      _eventAggregator.Publish(msg);

                                                                  }
                                                                  return;
                                                              case NotifyCollectionChangedAction.Move:
                                                              case NotifyCollectionChangedAction.Reset:
                                                              default:
                                                                  throw new ArgumentOutOfRangeException();
                                                          }
                                                      }

                                                      throw new NotSupportedException("Only supports StoryCardCollection or IterationCollection");

                                                      //CollectionChanged<TArtefact> collectionChanged =
                                                      //    new CollectionChanged<TArtefact>(x.Collection, x.Action,
                                                      //                                     x.NewItems, x.OldItems);

                                                      //_eventAggregator.Publish(collectionChanged);

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