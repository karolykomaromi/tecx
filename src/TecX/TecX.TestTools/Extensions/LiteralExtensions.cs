namespace TecX.TestTools.Extensions
{
    using System;

    using TecX.Common.Time;

    public static class LiteralExtensions
    {
        public static void Pass(this TimeSpan ts)
        {
            var previousTime = TimeProvider.UtcNow;

            (previousTime + ts).Freeze();
        }

        public static void Freeze(this DateTime dt)
        {
            TimeProvider timeProvider = new MockTimeProvider(dt);

            TimeProvider.Current = timeProvider;
        }
    }
}
