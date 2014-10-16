namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Text;

    public class DurationBuilder : Builder<string>
    {
        private TimeSpan duration;

        private int weeks;

        public DurationBuilder()
        {
            this.duration = -15.Minutes();
        }

        public DurationBuilder Weeks(int weeks)
        {
            this.weeks = weeks;

            return this;
        }

        public DurationBuilder Duration(TimeSpan duration)
        {
            this.duration = duration;

            return this;
        }

        public override string Build()
        {
            StringBuilder sb = new StringBuilder(50);

            if (this.weeks <= 0 && 
                this.duration < TimeSpan.Zero)
            {
                sb.Append(Constants.Before);
            }

            sb.Append("P");

            if (this.weeks != 0)
            {
                sb.Append(Math.Abs(this.weeks)).Append(Constants.Week);

                return sb.ToString();
            }

            //// modulus always positive = ((this.duration.Days % 7) + 7) % 7;

            int days = Math.Abs(this.duration.Days);

            if (days > 0)
            {
                sb.Append(days).Append(Constants.Day);
            }

            int hours = Math.Abs(this.duration.Hours);
            int minutes = Math.Abs(this.duration.Minutes);
            int seconds = Math.Abs(this.duration.Seconds);

            if (hours > 0 || minutes > 0 || seconds > 0)
            {
                sb.Append(Constants.Time);
            }

            if (hours != 0)
            {
                sb.Append(hours).Append(Constants.Hour);
            }

            if (minutes != 0)
            {
                sb.Append(minutes).Append(Constants.Minute);
            }

            if (seconds != 0)
            {
                sb.Append(seconds).Append(Constants.Second);
            }

            return sb.ToString();
        }

        private static class Constants
        {
            public const string Before = "-";

            public const string After = "+";

            public const string Week = "W";

            public const string Day = "D";

            public const string Hour = "H";

            public const string Minute = "M";

            public const string Second = "S";

            public const string Time = "T";
        }
    }
}