using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;

namespace TecX.Agile.Remote
{
    public class StoryCardMovedMessageFilter
    {
        private RingBuffer<Tuple<Guid, double, double, double>> _buffer;

        public StoryCardMovedMessageFilter()
        {
            _buffer = new RingBuffer<Tuple<Guid, double, double, double>>(10, 
                                                                          EqualityComparer<Tuple<Guid, double, double, double>>.Default);
        }

        public void Enqueue(Guid storyCardId, double x, double y, double angle)
        {
            _buffer.Add(new Tuple<Guid, double, double, double>(storyCardId, x, y, angle));
        }

        public bool ShouldLetPass(StoryCardMoved outboundMessage)
        {
            bool letPass = !_buffer.Remove(new Tuple<Guid, double, double, double>(
                                               outboundMessage.StoryCardId, outboundMessage.X, outboundMessage.Y, outboundMessage.Angle));

            return letPass;
        }
    }
}