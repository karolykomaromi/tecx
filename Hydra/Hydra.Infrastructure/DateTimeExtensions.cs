namespace Hydra.Infrastructure
{
    using System;

    public static class DateTimeExtensions
    {
        public static void Freeze(this DateTime dt)
        {
            TimeProvider.Current = new FrozenTimeProvider(dt);
        }
    }
}