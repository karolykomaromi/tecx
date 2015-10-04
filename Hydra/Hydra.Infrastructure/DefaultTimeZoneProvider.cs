namespace Hydra.Infrastructure
{
    using System;

    public class DefaultTimeZoneProvider : TimeZoneProvider
    {
        protected override TimeZoneInfo GetLocal()
        {
            return TimeZoneInfo.Local;
        }

        protected override TimeZoneInfo GetUtc()
        {
            return TimeZoneInfo.Utc;
        }
    }
}