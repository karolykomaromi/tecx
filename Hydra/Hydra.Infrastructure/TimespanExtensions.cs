namespace Hydra.Infrastructure
{
    using System;

    public static class TimeSpanExtensions
    {
        public static TimeSpan Milliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static TimeSpan Seconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        public static TimeSpan Hours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        public static TimeSpan Days(this int days)
        {
            return TimeSpan.FromDays(days);
        }

        public static TimeSpan Milliseconds(this double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        public static TimeSpan Seconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public static TimeSpan Minutes(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        public static TimeSpan Hours(this double hours)
        {
            return TimeSpan.FromHours(hours);
        }

        public static TimeSpan Days(this double days)
        {
            return TimeSpan.FromDays(days);
        }
    }
}
