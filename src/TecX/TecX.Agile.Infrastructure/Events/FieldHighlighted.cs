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

        public override string ToString()
        {
            return string.Format("FieldHighlighted Id:{0} Field:{1}", this.Id, this.FieldName);
        }
    }
}