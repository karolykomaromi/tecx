namespace TecX.Common.Time
{
    using System;

    public class DefaultTimeProvider : TimeProvider
    {
        protected override string GetString(DateTime dt)
        {
            // uses the roundtrip format string
            // ignores any FormatProvider
            return dt.ToString("o");
        }

        protected override DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        protected override DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}