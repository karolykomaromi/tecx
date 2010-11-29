using System;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class IncomingRequestToHighlightField : IDomainEvent
    {
        private readonly Guid _artefactId;
        private readonly string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public IncomingRequestToHighlightField(Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _artefactId = artefactId;
            _fieldName = fieldName;
        }
    }
}
