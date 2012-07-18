namespace TecX.Agile.Infrastructure.Events
{
    using System;

    public class DroppedStoryCard
    {
        public DroppedStoryCard(Guid id, double x, double y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
        }

        public Guid Id { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public override string ToString()
        {
            return string.Format("DroppedStoryCard Id:{0} X:{1} Y:{2}", this.Id, this.X, this.Y);
        }
    }
}
