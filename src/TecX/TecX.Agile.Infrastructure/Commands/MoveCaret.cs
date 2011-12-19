namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    public class MoveCaret
    {
        public MoveCaret(Guid artefactId, string fieldName, int caretIndex)
        {
            this.ArtefactId = artefactId;
            this.FieldName = fieldName;
            this.CaretIndex = caretIndex;
        }

        public Guid ArtefactId { get; private set; }

        public string FieldName { get; private set; }

        public int CaretIndex { get; private set; }
    }
}
