namespace TecX.Agile.Infrastructure.Events
{
    using System;

    using TecX.Common;

    public class CaretMoved
    {
        public CaretMoved(Guid id, string fieldName, int caretIndex)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            this.Id = id;
            this.FieldName = fieldName;
            this.CaretIndex = caretIndex;
        }

        public Guid Id { get; private set; }

        public string FieldName { get; private set; }

        public int CaretIndex { get; private set; }
    }
}