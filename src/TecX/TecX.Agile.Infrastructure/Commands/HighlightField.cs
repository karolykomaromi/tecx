namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    using TecX.Common;

    public class HighlightField
    {
        public HighlightField(Guid id, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            this.Id = id;
            this.FieldName = fieldName;
        }

        public Guid Id { get; private set; }

        public string FieldName { get; private set; }

        public override string ToString()
        {
            return string.Format("HighlightField Id:{0} Field:{1}", this.Id, this.FieldName);
        }
    }
}
