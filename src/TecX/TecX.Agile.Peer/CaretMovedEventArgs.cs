using System;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class CaretMovedEventArgs : EventArgs
    {
        private readonly Guid _artefactId;
        private readonly string _fieldName;
        private readonly int _caretIndex;
        private readonly Guid _senderId;

        public Guid SenderId
        {
            get { return _senderId; }
        }

        public int CaretIndex
        {
            get { return _caretIndex; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public CaretMovedEventArgs(Guid senderId, Guid artefactId, string fieldName, int caretIndex)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _senderId = senderId;
            _artefactId = artefactId;
            _fieldName = fieldName;
            _caretIndex = caretIndex;
        }
    }
}