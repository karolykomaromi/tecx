using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;

namespace TecX.Agile.Remote
{
    public class StoryCardMovedMessageFilter
    {
        private readonly RingBuffer<Tuple<Guid, double, double, double>> _buffer;

        public StoryCardMovedMessageFilter()
        {
            _buffer = new RingBuffer<Tuple<Guid, double, double, double>>(10, new StoryCardMovedComparer());
        }

        public void Enqueue(Guid storyCardId, double x, double y, double angle)
        {
            var tuple = new Tuple<Guid, double, double, double>(storyCardId, x, y, angle);

            _buffer.Add(tuple);
            _buffer.Add(tuple);
            _buffer.Add(tuple);
        }

        public bool ShouldLetPass(StoryCardMoved msg)
        {
            Guard.AssertNotNull(msg, "msg");

            var tuple = new Tuple<Guid, double, double, double>(msg.StoryCardId, msg.X, msg.Y, msg.Angle);

            bool letPass = !_buffer.Remove(tuple);

            return letPass;
        }

        private class StoryCardMovedComparer : IEqualityComparer<Tuple<Guid, double, double, double>>
        {
            public bool Equals(Tuple<Guid, double, double, double> x, Tuple<Guid, double, double, double> y)
            {
                Guard.AssertNotNull(x, "x");
                Guard.AssertNotNull(y, "y");

                if (x.Item1 != y.Item1)
                    return false;

                if (x.Item2 == y.Item2)
                    return true;
                
                if (x.Item3 == y.Item3)
                    return true;
                
                if (x.Item4 == y.Item4)
                    return true;

                return false;
            }

            public int GetHashCode(Tuple<Guid, double, double, double> obj)
            {
                Guard.AssertNotNull(obj, "obj");

                return obj.Item1.GetHashCode();
            }
        }
    }
}