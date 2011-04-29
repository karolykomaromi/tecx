using System;
using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class IterationReplaced : IDomainEvent
    {
        [DataMember]
        private readonly Guid _oldItemId;
        [DataMember]
        private readonly Guid _newItemId;
        [DataMember]
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