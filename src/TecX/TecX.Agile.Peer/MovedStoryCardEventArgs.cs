using System;

namespace TecX.Agile.Peer
{
    public class MovedStoryCardEventArgs : EventArgs
    {
        public Guid Id { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public double Angle { get; set; }
    }
}