namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Text;

    public class Duration : CalendarItem<Duration>
    {
        public Duration()
        {
            this.Time = -15.Minutes();
        }

        public int Weeks { get; set; }

        public TimeSpan Time { get; set; }

        public override Duration Clone()
        {
            var clone = new Duration { Weeks = this.Weeks, Time = this.Time };

            return clone;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(50);

            if (this.Weeks <= 0 &&
                this.Time < TimeSpan.Zero)
            {
                sb.Append(Constants.Before);
            }

            sb.Append("P");

            if (this.Weeks != 0)
            {
                sb.Append(Math.Abs(this.Weeks)).Append(Constants.Week);

                return sb.ToString();
            }

            //// modulus always positive = ((this.duration.Days % 7) + 7) % 7;

            int days = Math.Abs(this.Time.Days);

            if (days > 0)
            {
                sb.Append(days).Append(Constants.Day);
            }

            int hours = Math.Abs(this.Time.Hours);
            int minutes = Math.Abs(this.Time.Minutes);
            int seconds = Math.Abs(this.Time.Seconds);

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