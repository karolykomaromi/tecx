using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class CaretMoved : IDomainEvent
    {
        private readonly Guid _artefactId;
        private readonly int _caretIndex;

        public int CaretIndex
        {
            get { return _caretIndex; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public CaretMoved(Guid artefactId, int caretIndex)
        {
            _artefactId = artefactId;
            _caretIndex = caretIndex;
        }
    }
}