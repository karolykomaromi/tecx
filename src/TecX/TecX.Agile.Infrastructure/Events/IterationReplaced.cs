using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class IterationReplaced : IDomainEvent
    {
        private readonly Guid _oldItemId;
        private readonly Guid _newItemId;
        private readonly Guid _collectionId;

        public Guid CollectionId
        {
            get { return _collectionId; }
        }

        public Guid NewItemId
        {
            get { return _newItemId; }
        }

        public Guid OldItemId
        {
            get { return _oldItemId; }
        }


        public IterationReplaced(Guid oldItemId, Guid newItemId, Guid collectionId)
        {
            _oldItemId = oldItemId;
            _newItemId = newItemId;
            _collectionId = collectionId;
        }
    }
}