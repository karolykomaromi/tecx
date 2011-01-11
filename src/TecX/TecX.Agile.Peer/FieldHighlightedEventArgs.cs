using System;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class FieldHighlightedEventArgs : EventArgs
    {
        private readonly Guid _senderId;
        private readonly Guid _artefactId;
        private readonly string _fieldName;

        public Guid SenderId
        {
            get { return _senderId; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public FieldHighlightedEventArgs(Guid senderId, Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _senderId = senderId;
            _artefactId = artefactId;
            _fieldName = fieldName;
        }
    }
}
