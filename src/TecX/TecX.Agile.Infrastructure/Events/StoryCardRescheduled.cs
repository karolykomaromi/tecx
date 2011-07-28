using System;
using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class StoryCardRescheduled : IDomainEvent
    {
        #region Fields

        [DataMember]
        private readonly Guid _storyCardId;
        [DataMember]
        private readonly Guid _from;
        [DataMember]
        private readonly Guid _to;

        #endregion Fields

        #region Properties

        public Guid To
        {
            get { return _to; }
        }

        public Guid From
        {
            get { return _from; }
        }

        public Guid StoryCardId
        {
            get { return _storyCardId; }
        }

        #endregion Properties

        #region c'tor

        public StoryCardRescheduled(Guid storyCardId, Guid from, Guid to)
        {
            _storyCardId = storyCardId;
            _from = from;
            _to = to;
        }

        #endregion c'tor
    }
}