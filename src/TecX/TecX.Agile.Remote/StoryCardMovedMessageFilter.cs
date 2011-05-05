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
            public bool Equals(StoryCardMoved a, StoryCardMoved b)
            {
                Guard.AssertNotNull(a, "a");
                Guard.AssertNotNull(b, "b");

                if (a.StoryCardId != b.StoryCardId)
                    return false;

                if (a.To.X == b.To.X)
                    return true;
                
                if (a.To.Y == b.To.Y)
                    return true;
                
                if (a.To.Angle == b.To.Angle)
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