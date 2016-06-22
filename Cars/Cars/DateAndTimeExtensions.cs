namespace Cars
{
    using System;

    public static class DateAndTimeExtensions
    {
        public static DateTimeOffset Freeze(this DateTimeOffset dt)
        {
            TimeProvider.Current = new FrozenTimeProvider(dt);

            return dt;
        }
    }
}