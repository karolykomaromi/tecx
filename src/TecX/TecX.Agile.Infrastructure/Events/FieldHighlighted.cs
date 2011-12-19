namespace TecX.Agile.Infrastructure.Events
{
    using System;

    using TecX.Common;

    public class FieldHighlighted
    {
        public FieldHighlighted(Guid id, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            this.Id = id;
            this.FieldName = fieldName;
        }

        public Guid Id { get; private set; }

        public string FieldName { get; private set; }
    }
}