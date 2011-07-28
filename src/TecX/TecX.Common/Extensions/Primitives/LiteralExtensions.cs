using System;

namespace TecX.Common.Extensions.Primitives
{
    public static class LiteralExtensions
    {
        public static TimeSpan Milliseconds(this int ts)
        {
            return TimeSpan.FromMilliseconds(ts);
        }

        public static TimeSpan Seconds(this int ts)
        {
            return TimeSpan.FromSeconds(ts);
        }

        public static TimeSpan Minutes(this int ts)
        {
            return TimeSpan.FromMinutes(ts);
        }

        public static TimeSpan Hours(this int ts)
        {
            return TimeSpan.FromHours(ts);
        }

        public static TimeSpan Days(this int ts)
        {
            return TimeSpan.FromDays(ts);
        }

        public static TimeSpan Milliseconds(this double ts)
        {
            return TimeSpan.FromMilliseconds(ts);
        }

        public static TimeSpan Seconds(this double ts)
        {
            return TimeSpan.FromSeconds(ts);
        }

        public static TimeSpan Minutes(this double ts)
        {
            return TimeSpan.FromMinutes(ts);
        }

        public static TimeSpan Hours(this double ts)
        {
            return TimeSpan.FromHours(ts);
        }

        public static TimeSpan Days(this double ts)
        {
            return TimeSpan.FromDays(ts);
        }
    }
}
