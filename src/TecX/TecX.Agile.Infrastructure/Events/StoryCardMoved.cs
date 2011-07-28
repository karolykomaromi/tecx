using System;
using System.Runtime.Serialization;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class StoryCardMoved : IDomainEvent
    {
        #region Fields

        [DataMember]
        private readonly Guid _storyCardId;
        [DataMember]
        private readonly PositionAndOrientation _from;
        [DataMember]
        private readonly PositionAndOrientation _to;

        #endregion Fields

        #region Properties

        public PositionAndOrientation From
        {
            get { return _from; }
        }

        public PositionAndOrientation To
        {
            get { return _to; }
        }

        public Guid StoryCardId
        {
            get { return _storyCardId; }
        }

        #endregion Properties

        #region c'tor

        public StoryCardMoved(Guid storyCardId, PositionAndOrientation from, PositionAndOrientation to)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            _storyCardId = storyCardId;
            _from = from;
            _to = to;
        }

        #endregion c'tor
    }
}
