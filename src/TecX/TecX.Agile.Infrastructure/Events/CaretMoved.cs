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

        public override string ToString()
        {
            return string.Format("CaretMoved Id:{0} Field:{1} Idx:{1}", this.Id, this.FieldName, this.CaretIndex);
        }
    }
}