using System;

namespace TecX.Agile.Peer
{
    public class CaretMovedEventArgs : EventArgs
    {
        public Guid ArtefactId { get; set; }

        public int CaretIndex { get; set; }
    }
}