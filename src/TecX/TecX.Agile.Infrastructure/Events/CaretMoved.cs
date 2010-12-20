using System;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class CaretMoved : IDomainEvent
    {
        private readonly Guid _artefactId;
        private readonly string _fieldName;
        private readonly int _caretIndex;

        public string FieldName
        {
            get { return _fieldName; }
        }

        public int CaretIndex
        {
            get { return _caretIndex; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public CaretMoved(Guid artefactId, string fieldName, int caretIndex)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _artefactId = artefactId;
            _fieldName = fieldName;
            _caretIndex = caretIndex;
        }
    }
}