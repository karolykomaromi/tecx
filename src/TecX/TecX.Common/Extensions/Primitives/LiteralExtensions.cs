namespace TecX.Common.Extensions.Primitives
{
    using System;
    using System.Diagnostics;

    public static class LiteralExtensions
    {
        [DebuggerStepThrough]
        public static TimeSpan Milliseconds(this int ts)
        {
            return TimeSpan.FromMilliseconds(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Seconds(this int ts)
        {
            return TimeSpan.FromSeconds(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Minutes(this int ts)
        {
            return TimeSpan.FromMinutes(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Hours(this int ts)
        {
            return TimeSpan.FromHours(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Days(this int ts)
        {
            return TimeSpan.FromDays(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Milliseconds(this double ts)
        {
            return TimeSpan.FromMilliseconds(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Seconds(this double ts)
        {
            return TimeSpan.FromSeconds(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Minutes(this double ts)
        {
            return TimeSpan.FromMinutes(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Hours(this double ts)
        {
            return TimeSpan.FromHours(ts);
        }

        [DebuggerStepThrough]
        public static TimeSpan Days(this double ts)
        {
            return TimeSpan.FromDays(ts);
        }
    }
}
