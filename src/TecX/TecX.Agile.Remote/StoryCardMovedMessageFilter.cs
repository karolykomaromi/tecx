using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;
using TecX.Common.Extensions.Primitives;

namespace TecX.Agile.Remote
{
    public class StoryCardMovedMessageFilter : IMessageFilter<StoryCardMoved>
    {
        private readonly MessageHistory<StoryCardMoved> _messageHistory;

        public StoryCardMovedMessageFilter()
        {
            _messageHistory = new ExpiringMessageHistory<StoryCardMoved>(1.Minutes(), new StoryCardMovedComparer());
        }

        public void Enqueue(StoryCardMoved @event)
        {
            Guard.AssertNotNull(@event, "event");

            _messageHistory.Add(@event);
        }

        public bool ShouldLetPass(StoryCardMoved msg)
        {
            Guard.AssertNotNull(msg, "msg");
            
            bool letPass = !_messageHistory.Contains(msg);

            return letPass;
        }

        private class StoryCardMovedComparer : IEqualityComparer<StoryCardMoved>
        {
            public bool Equals(StoryCardMoved x, StoryCardMoved y)
            {
                Guard.AssertNotNull(x, "x");
                Guard.AssertNotNull(y, "y");

                if (x.StoryCardId != y.StoryCardId)
                    return false;

                if (x.X == y.X)
                    return true;
                
                if (x.Y == y.Y)
                    return true;
                
                if (x.Angle == y.Angle)
                    return true;

                return false;
            }

            public int GetHashCode(StoryCardMoved obj)
            {
                Guard.AssertNotNull(obj, "obj");

                return obj.StoryCardId.GetHashCode();
            }
        }
    }
}