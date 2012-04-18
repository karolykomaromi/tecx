﻿namespace TecX.TestTools.Extensions
{
    using System;

    using Moq;

    using TecX.Common.Time;

    public static class LiteralExtensions
    {
        public static void Pass(this TimeSpan ts)
        {
            var previousTime = TimeProvider.Current.GetUtcNow();

            (previousTime + ts).Freeze();
        }

        public static void Freeze(this DateTime dt)
        {
            var timeProviderStub = new Mock<TimeProvider>();

            timeProviderStub.SetupGet(tp => tp.GetUtcNow()).Returns(dt);

            TimeProvider.Current = timeProviderStub.Object;
        }
    }
}
