namespace TecX.Agile.Infrastructure.Events
{
    using System;

    public class DroppedStoryCard
    {
        public DroppedStoryCard(Guid id, double x, double y)
        {
            this.Id = id;
            this.AbsoluteX = x;
            this.AbsoluteY = y;
        }

        public Guid Id { get; set; }

        public double AbsoluteX { get; set; }

        public double AbsoluteY { get; set; }
    }
}
