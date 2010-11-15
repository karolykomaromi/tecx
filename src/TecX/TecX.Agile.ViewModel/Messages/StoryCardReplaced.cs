using TecX.Common;

namespace TecX.Agile.ViewModel.Messages
{
    public class StoryCardReplaced : IMessage
    {
        private readonly StoryCard _oldItem;
        private readonly StoryCard _newItem;
        private readonly StoryCardCollection _collection;

        public StoryCardCollection Collection
        {
            get { return _collection; }
        }

        public StoryCard NewItem
        {
            get { return _newItem; }
        }

        public StoryCard OldItem
        {
            get { return _oldItem; }
        }

        public StoryCardReplaced(StoryCard oldItem, StoryCard newItem, StoryCardCollection collection)
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