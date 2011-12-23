namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    public class AddStoryCard
    {
        public AddStoryCard(Guid id, double x, double y, double angle)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Angle = angle;
        }

        public Guid Id { get; private set; }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Angle { get; private set; }

        public override string ToString()
        {
            return string.Format("AddStoryCard Id:{0} X:{1} Y:{2} Angle:{3}", this.Id, this.X, this.Y, this.Angle);
        }
    }
}
