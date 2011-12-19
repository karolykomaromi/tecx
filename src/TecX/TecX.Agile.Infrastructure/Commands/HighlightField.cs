namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    using TecX.Common;

    public class HighlightField
    {
        public HighlightField(Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            this.ArtefactId = artefactId;
            this.FieldName = fieldName;
        }

        public Guid ArtefactId { get; private set; }

        public string FieldName { get; private set; }
    }
}
