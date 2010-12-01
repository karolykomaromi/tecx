using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;

namespace TecX.Agile.Remote
{
    public class PropertyChangedMessageFilter
    {
        private readonly RingBuffer<Tuple<Guid, string, object, object>> _buffer;

        public PropertyChangedMessageFilter()
        {
            _buffer = new RingBuffer<Tuple<Guid, string, object, object>>(10, 
                                                                          EqualityComparer<Tuple<Guid, string, object, object>>.Default);
        }

        public void Enqueue(Guid storyCardId, string propertyName, object oldValue, object newValue)
        {
            _buffer.Add(new Tuple<Guid, string, object, object>(storyCardId, propertyName, oldValue, newValue));
        }

        public bool ShouldLetPass(PropertyUpdated outboundMessage)
        {
            Guard.AssertNotNull(outboundMessage, "outboundMessage");
            Guard.AssertNotEmpty((string) outboundMessage.PropertyName, "outboundMessage.PropertyName");

            bool letPass =
                !_buffer.Remove(new Tuple<Guid, string, object, object>(outboundMessage.ArtefactId,
                                                                        outboundMessage.PropertyName,
                                                                        outboundMessage.OldValue,
                                                                        outboundMessage.NewValue));

            return letPass;
        }
    }
}