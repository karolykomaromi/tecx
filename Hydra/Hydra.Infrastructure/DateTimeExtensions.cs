namespace Hydra.Infrastructure
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime Freeze(this DateTime dt)
        {
            TimeProvider.Current = new FrozenTimeProvider(dt);

            return dt;
        }
    }
}