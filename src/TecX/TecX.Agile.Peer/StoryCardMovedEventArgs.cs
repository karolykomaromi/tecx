using System;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;

namespace TecX.Agile.Peer
{
    public class StoryCardMovedEventArgs : EventArgs
    {
        private readonly Guid _senderId;
        private readonly Guid _storyCardId;
        private readonly PositionAndOrientation _from;
        private readonly PositionAndOrientation _to;

        public Guid SenderId
        {
            get { return _senderId; }
        }

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

        public StoryCardMovedEventArgs(Guid senderId, Guid storyCardId, PositionAndOrientation from, PositionAndOrientation to)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            _senderId = senderId;
            _storyCardId = storyCardId;
            _from = from;
            _to = to;
        }
    }
}