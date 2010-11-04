using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel
{
    public class CollectionChangedAction<TArtefact> : AbstractAction
        where TArtefact : PlanningArtefact
    {
        private readonly PlanningArtefactCollection<TArtefact> _collection;
        private readonly IEnumerable<TArtefact> _newItems;
        private readonly IEnumerable<TArtefact> _oldItems;
        private readonly NotifyCollectionChangedAction _action;


        public PlanningArtefactCollection<TArtefact> Collection
        {
            get { return _collection; }
        }

        public IEnumerable<TArtefact> NewItems
        {
            get { return _newItems; }
        }

        public IEnumerable<TArtefact> OldItems
        {
            get { return _oldItems; }
        }

        public NotifyCollectionChangedAction Action
        {
            get { return _action; }
        }

        public CollectionChangedAction(PlanningArtefactCollection<TArtefact> collection,
            NotifyCollectionChangedAction action,
            IEnumerable<TArtefact> newItems,
            IEnumerable<TArtefact> oldItems)
        {
            Guard.AssertNotNull(collection, "collection");
            Guard.AssertNotNull(newItems, "newItems");
            Guard.AssertNotNull(oldItems, "oldItems");

            _collection = collection;
            _action = action;
            _newItems = newItems;
            _oldItems = oldItems;

        }

        protected override void UnExecuteCore()
        {
            switch (Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (TArtefact item in NewItems)
                        {
                            Collection.Remove(item.Id);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {

                        foreach (TArtefact item in OldItems)
                        {
                            if (!Collection.Contains(item))
                                Collection.Add(item);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        TArtefact oldItem = OldItems.First();
                        TArtefact newItem = NewItems.First();

                        Collection[newItem.Id] = oldItem;

                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void ExecuteCore()
        {
            switch (Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (TArtefact item in NewItems)
                        {
                            if (!Collection.Contains(item))
                                Collection.Add(item);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (TArtefact item in OldItems)
                        {
                            Collection.Remove(item.Id);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        TArtefact oldItem = OldItems.First();
                        TArtefact newItem = NewItems.First();

                        Collection[oldItem.Id] = newItem;

                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}