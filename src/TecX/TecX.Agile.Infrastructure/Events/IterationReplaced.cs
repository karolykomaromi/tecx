using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class IterationReplaced : IDomainEvent
    {
        private readonly Iteration _oldItem;
        private readonly Iteration _newItem;
        private readonly IterationCollection _collection;

        public IterationCollection Collection
        {
            get { return _collection; }
        }

        public Iteration NewItem
        {
            get { return _newItem; }
        }

        public Iteration OldItem
        {
            get { return _oldItem; }
        }


        public IterationReplaced(Iteration oldItem, Iteration newItem, IterationCollection collection)
        {
            Guard.AssertNotNull(oldItem, "oldItem");
            Guard.AssertNotNull(newItem, "newItem");
            Guard.AssertNotNull(collection, "collection");

            _oldItem = oldItem;
            _newItem = newItem;
            _collection = collection;
        }
    }
}