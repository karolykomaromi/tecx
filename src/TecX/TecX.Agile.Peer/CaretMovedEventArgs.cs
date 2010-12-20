using System;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class CaretMovedEventArgs : EventArgs
    {
        private readonly Guid _artefactId;
        private readonly string _fieldName;
        private readonly int _caretIndex;

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

        public CaretMovedEventArgs(Guid artefactId, string fieldName, int caretIndex)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _artefactId = artefactId;
            _fieldName = fieldName;
            _caretIndex = caretIndex;
        }
    }
}