using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;
using TecX.Common.Extensions.Primitives;

namespace TecX.Agile.Remote
{
    public class HighlightMessageFilter : IMessageFilter<FieldHighlighted>
    {
        private readonly MessageHistory<FieldHighlighted> _messageHistory;

        public HighlightMessageFilter()
        {
            this._messageHistory = new ExpiringMessageHistory<FieldHighlighted>(1.Minutes(), new FieldHighlightedEqualityComparer());
        }

        public void Enqueue(FieldHighlighted inboundMessage)
        {
            Guard.AssertNotNull(inboundMessage, "inboundMessage");

            _messageHistory.Add(inboundMessage);
        }

        public bool ShouldLetPass(FieldHighlighted outboundMessage)
        {
            Guard.AssertNotNull(outboundMessage, "outboundMessage");
            Guard.AssertNotEmpty(outboundMessage.FieldName, "outboundMessage.FieldName");

            bool letPass = !_messageHistory.Contains(outboundMessage);

            return letPass;
        }

        private class FieldHighlightedEqualityComparer : EqualityComparer<FieldHighlighted>
        {
            public override bool Equals(FieldHighlighted x, FieldHighlighted y)
            {
                Guard.AssertNotNull(x, "x");
                Guard.AssertNotNull(y, "y");
                Guard.AssertNotEmpty(x.FieldName, "x.FieldName");
                Guard.AssertNotEmpty(y.FieldName, "y.FieldName");

                if(x.ArtefactId != y.ArtefactId)
                {
                    return false;
                }

                return string.Equals(x.FieldName, y.FieldName, StringComparison.InvariantCultureIgnoreCase);
            }

            public override int GetHashCode(FieldHighlighted obj)
            {
                Guard.AssertNotNull(obj, "obj");

                return obj.ArtefactId.GetHashCode();
            }
        }
    }
}