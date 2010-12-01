using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardReplaced : IDomainEvent
    {
        private readonly Guid _oldItemId;
        private readonly Guid _newItemId;
        private readonly Guid _collection;

        public Guid Collection
        {
            get { return _collection; }
        }

        public Guid NewItemId
        {
            get { return _newItemId; }
        }

        public Guid OldItemId
        {
            get { return _oldItemId; }
        }

        public StoryCardReplaced(Guid oldItemId, Guid newItemId, Guid collection)
        {
            _oldItemId = oldItemId;
            _newItemId = newItemId;
            _collection = collection;
        }
    }
}