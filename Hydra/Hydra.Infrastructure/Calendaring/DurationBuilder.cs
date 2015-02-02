namespace Hydra.Infrastructure.Calendaring
{
    using System;

    public class DurationBuilder : Builder<Duration>
    {
        private readonly Duration duration;

        public DurationBuilder()
        {
            this.duration = new Duration();
        }

        public DurationBuilder Weeks(int weeks)
        {
            this.duration.Weeks = weeks;

            return this;
        }

        public DurationBuilder Duration(TimeSpan duration)
        {
            this.duration.Time = duration;

            return this;
        }

        public override Duration Build()
        {
            return this.duration.Clone();
        }
    }
}