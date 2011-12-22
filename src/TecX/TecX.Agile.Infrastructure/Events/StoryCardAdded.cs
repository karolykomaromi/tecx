namespace TecX.Agile.Infrastructure.Events
{
    using System;

    public class StoryCardAdded
    {
        public StoryCardAdded(Guid id, double x, double y, double angle)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Angle = angle;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Angle { get; private set; }

        public Guid Id { get; private set; }
    }
}
