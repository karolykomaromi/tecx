using System;

namespace TecX.Agile.Peer
{
    public class StoryCardMovedEventArgs : EventArgs
    {
        public Guid StoryCardId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Angle { get; set; }
    }
}