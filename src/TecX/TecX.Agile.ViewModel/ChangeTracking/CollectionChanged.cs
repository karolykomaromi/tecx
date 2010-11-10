using System.Collections.Generic;
using System.Collections.Specialized;

using TecX.Common;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class CollectionChanged<TArtefact> : IMessage
        where TArtefact : PlanningArtefact
    {
        #region Fields

        private readonly PlanningArtefactCollection<TArtefact> _collection;
        private readonly NotifyCollectionChangedAction _action;
        private readonly IEnumerable<TArtefact> _newItems;
        private readonly IEnumerable<TArtefact> _oldItems;

        #endregion Fields

        #region Properties

        public IEnumerable<TArtefact> OldItems
        {
            get { return _oldItems; }
        }

        public IEnumerable<TArtefact> NewItems
        {
            get { return _newItems; }
        }

        public NotifyCollectionChangedAction Action1
        {
            get { return _action; }
        }

        public PlanningArtefactCollection<TArtefact> Collection
        {
            get { return _collection; }
        }

        #endregion Properties

        #region c'tor

        public CollectionChanged(PlanningArtefactCollection<TArtefact> collection, NotifyCollectionChangedAction action, 
                                 IEnumerable<TArtefact> newItems, IEnumerable<TArtefact> oldItems)
        {
            Guard.AssertNotNull(collection, "collection");
            Guard.AssertNotNull(newItems, "newItems");
            Guard.AssertNotNull(oldItems, "oldItems");

            _collection = collection;
            _action = action;
            _newItems = newItems;
            _oldItems = oldItems;
        }

        #endregion c'tor
    }
}