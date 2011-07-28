using System;
using System.Runtime.Serialization;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class FieldHighlighted : IDomainEvent
    {        
        [DataMember]
        private readonly Guid _artefactId;
        [DataMember]
        private readonly string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public FieldHighlighted(Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _artefactId = artefactId;
            _fieldName = fieldName;
        }
    }
}
