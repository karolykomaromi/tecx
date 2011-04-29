using System;
using System.Runtime.Serialization;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class CaretMoved : IDomainEvent
    {
        [DataMember]
        private readonly Guid _artefactId;
        [DataMember]
        private readonly string _fieldName;
        [DataMember]
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