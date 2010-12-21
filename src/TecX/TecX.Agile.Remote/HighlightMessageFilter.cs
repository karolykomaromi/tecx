﻿using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Peer;
using TecX.Common;

namespace TecX.Agile.Remote
{
    public class HighlightMessageFilter
    {
        private readonly RingBuffer<Tuple<Guid, string>> _buffer;

        public HighlightMessageFilter()
        {
            _buffer = new RingBuffer<Tuple<Guid, string>>(10, EqualityComparer<Tuple<Guid, string>>.Default);
        }

        public void Enqueue(Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _buffer.Add(new Tuple<Guid, string>(artefactId, fieldName));
        }

        public bool ShouldLetPass(FieldHighlighted outboundMessage)
        {
            Guard.AssertNotNull(outboundMessage, "outboundMessage");
            Guard.AssertNotEmpty((string) outboundMessage.FieldName, "outboundMessage.FieldName");

            bool letPass = !_buffer.Remove(new Tuple<Guid, string>(outboundMessage.ArtefactId, outboundMessage.FieldName));

            return letPass;
        }
    }
}