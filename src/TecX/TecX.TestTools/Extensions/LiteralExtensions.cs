using System;

using Moq;

using TecX.Common.Time;

namespace TecX.TestTools.Extensions
{
    public static class LiteralExtensions
    {
        public static void Times(this int i, Action action)
        {
            for(int j = 0; j < i; j++)
            {
                action();
            }
        }

        public static void Pass(this TimeSpan ts)
        {
            var previousTime = TimeProvider.Current.UtcNow;

            (previousTime + ts).Freeze();
        }

        public static void Freeze(this DateTime dt)
        {
            var timeProviderStub = new Mock<TimeProvider>();

            timeProviderStub.SetupGet(tp => tp.UtcNow).Returns(dt);

            TimeProvider.Current = timeProviderStub.Object;
        }
    }
}
