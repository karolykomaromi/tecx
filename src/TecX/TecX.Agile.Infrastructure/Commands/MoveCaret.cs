namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    public class MoveCaret
    {
        public MoveCaret(Guid id, string fieldName, int caretIndex)
        {
            this.Id = id;
            this.FieldName = fieldName;
            this.CaretIndex = caretIndex;
        }

        public Guid Id { get; private set; }

        public string FieldName { get; private set; }

        public int CaretIndex { get; private set; }

        public override string ToString()
        {
            return string.Format("MoveCaret Id:{0} Field:{1} Idx:{2}", this.Id, this.FieldName, this.CaretIndex);
        }
    }
}
