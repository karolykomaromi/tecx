using System;
using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class StoryCardReplaced : IDomainEvent
    {
        [DataMember]
        private readonly Guid _oldItemId;
        [DataMember]
        private readonly Guid _newItemId;
        [DataMember]
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