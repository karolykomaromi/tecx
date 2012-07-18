using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;
using TecX.Common.Comparison;
using TecX.Common.Extensions.Primitives;

namespace TecX.Agile.Remote
{
    public class PropertyChangedMessageFilter : IMessageFilter<PropertyUpdated>
    {
        private readonly MessageHistory<PropertyUpdated> _messageHistory;

        public PropertyChangedMessageFilter()
        {
            _messageHistory = new ExpiringMessageHistory<PropertyUpdated>(1.Minutes(), new PropertyUpdatedEqualityComparer());
        }

        public void Enqueue(PropertyUpdated inboundMessage)
        {
            Guard.AssertNotNull(inboundMessage, "inboundMessage");

            _messageHistory.Add(inboundMessage);
        }

        public bool ShouldLetPass(PropertyUpdated outboundMessage)
        {
            Guard.AssertNotNull(outboundMessage, "outboundMessage");
            Guard.AssertNotEmpty((string) outboundMessage.PropertyName, "outboundMessage.PropertyName");

            bool letPass = !_messageHistory.Contains(outboundMessage);

            return letPass;
        }

        private class PropertyUpdatedEqualityComparer : EqualityComparer<PropertyUpdated>
        {
            public override bool Equals(PropertyUpdated x, PropertyUpdated y)
            {
                Guard.AssertNotNull(x, "x");
                Guard.AssertNotNull(y, "y");
                Guard.AssertNotEmpty(x.PropertyName, "x.PropertyName");
                Guard.AssertNotEmpty(y.PropertyName, "y.PropertyName");

                if(x.ArtefactId != y.ArtefactId)
                {
                    return false;
                }

                if(!string.Equals(x.PropertyName, y.PropertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                if(!Compare.AreEqual(x.NewValue, y.NewValue))
                {
                    return false;
                }

                if(!Compare.AreEqual(x.OldValue, y.OldValue))
                {
                    return false;
                }

                return true;
            }

            public override int GetHashCode(PropertyUpdated obj)
            {
                Guard.AssertNotNull(obj, "obj");

                return obj.ArtefactId.GetHashCode();
            }
        }
    }
}