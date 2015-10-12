namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Diagnostics.Contracts;

    public static class DateAndTimeExtensions
    {
        public static DateTime Freeze(this DateTime dt)
        {
            TimeProvider.Current = new FrozenTimeProvider(dt);

            return dt;
        }

        public static DateTimeOffset ToOffset(this DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Local || dt.Kind == DateTimeKind.Unspecified)
            {
                return ToOffset(dt, TimeZoneProvider.Local);
            }

            return ToOffset(dt, TimeZoneProvider.Utc);
        }

        public static DateTimeOffset ToOffset(this DateTime dt, TimeZoneInfo timeZone)
        {
            Contract.Requires(timeZone != null);

            return new DateTimeOffset(dt, timeZone.BaseUtcOffset);
        }
    }
}