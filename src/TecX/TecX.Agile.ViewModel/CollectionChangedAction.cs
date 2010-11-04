using System;
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
        private readonly NotifyCollectionChangedEventArgs _args;

        public NotifyCollectionChangedEventArgs EventArgs
        {
            get { return _args; }
        }

        public PlanningArtefactCollection<TArtefact> Collection
        {
            get { return _collection; }
        }

        public CollectionChangedAction(PlanningArtefactCollection<TArtefact> collection, NotifyCollectionChangedEventArgs args)
        {
            Guard.AssertNotNull(collection, "collection");
            Guard.AssertNotNull(args, "args");

            _collection = collection;
            _args = args;
        }

        protected override void UnExecuteCore()
        {
            switch (_args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (TArtefact item in _args.OldItems)
                        {
                            Collection.Remove(item.Id);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {

                        foreach (TArtefact item in _args.NewItems)
                        {
                            Collection.Add(item);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        TArtefact oldItem = (TArtefact) _args.OldItems[0];
                        TArtefact newItem = (TArtefact) _args.NewItems[0];

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
            switch (_args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach(TArtefact item in _args.NewItems)
                        {
                            Collection.Add(item);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach(TArtefact item in _args.OldItems)
                        {
                            Collection.Remove(item.Id);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        TArtefact oldItem = (TArtefact) _args.OldItems[0];
                        TArtefact newItem = (TArtefact) _args.NewItems[0];

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