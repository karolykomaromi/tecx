namespace Cars
{
    using System;

    public class DefaultTimeProvider : TimeProvider
    {
        protected override DateTimeOffset GetNow()
        {
            return DateTimeOffset.Now;
        }

        protected override DateTimeOffset GetUtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}