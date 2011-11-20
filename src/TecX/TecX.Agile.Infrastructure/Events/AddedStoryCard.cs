namespace TecX.Agile.Infrastructure.Events
{
    using System;

    public class AddedStoryCard
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Angle { get; set; }

        public Guid Id { get; set; }
    }
}
