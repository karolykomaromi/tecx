using System;
using System.Collections.Specialized;
using System.Linq;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel
{
    public class CollectionChangeSubscription<TArtefact> : IChangeSubscription
        where TArtefact : PlanningArtefact
    {
        private readonly PlanningArtefactCollection<TArtefact> _collection;
        private readonly IActionManager _actionManager;
        private readonly IDisposable _subscription;

        public CollectionChangeSubscription(IActionManager actionManager, PlanningArtefactCollection<TArtefact> collection)
        {
            Guard.AssertNotNull(actionManager, "actionManager");
            Guard.AssertNotNull(collection, "collection");

            _actionManager = actionManager;
            _collection = collection;

            var changed = from evt in Observable.FromEvent<NotifyCollectionChangedEventArgs>(collection, "CollectionChanged")
                          select
                              new
                                  {
                                      Collection = (PlanningArtefactCollection<TArtefact>)evt.Sender,
                                      evt.EventArgs
                                  };

            _subscription = changed.Subscribe(x =>
                                  {
                                      var action = new AddItemToCollectionAction<TArtefact>(x.Collection,
                                                                                            (TArtefact)x.EventArgs.NewItems[0]);

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
            if (disposing)
            {
                _subscription.Dispose();
            }
        }
    }
}