using System;
using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class StoryCardPostponed : IDomainEvent
    {
        [DataMember]
        private readonly Guid _storyCardId;
        [DataMember]
        private readonly Guid _from;

        public Guid From
        {
            get { return _from; }
        }

        public Guid StoryCardId
        {
            get { return _storyCardId; }
        }

        public StoryCardPostponed(Guid storyCardId, Guid from)
        {
            _storyCardId = storyCardId;
            _from = from;
        }

    }
}