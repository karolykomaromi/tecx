using System.Linq;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class AddItemToCollectionAction<TArtefact> : Undo.AbstractAction
        where TArtefact : PlanningArtefact
    {
        private readonly PlanningArtefactCollection<TArtefact> _collection;
        private readonly TArtefact _item;

        public TArtefact Item
        {
            get { return _item; }
        }

        public PlanningArtefactCollection<TArtefact> Collection
        {
            get { return _collection; }
        }

        public AddItemToCollectionAction(PlanningArtefactCollection<TArtefact> collection, TArtefact item)
        {
            Guard.AssertNotNull(collection, "collection");
            Guard.AssertNotNull(item, "item");

            _collection = collection;
            _item = item;
        }

        protected override void UnExecuteCore()
        {
            _collection.Remove(_item.Id);
        }

        protected override void ExecuteCore()
        {
            if (!_collection.Contains(_item))
                _collection.Add(_item);
        }
    }
}