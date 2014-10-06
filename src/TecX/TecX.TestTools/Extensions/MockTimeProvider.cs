namespace TecX.TestTools.Extensions
{
    using System;

    using TecX.Common.Time;

    public class MockTimeProvider : TimeProvider
    {
        private readonly DateTime now;

        private readonly DateTime utcNow;

        public MockTimeProvider(DateTime now)
            : this(now, now)
        {
        }

        public MockTimeProvider(DateTime now, DateTime utcNow)
        {
            this.now = now;
            this.utcNow = utcNow;
        }

        protected override DateTime GetNow()
        {
            return this.now;
        }

        protected override string GetString(DateTime dt)
        {
            return dt.ToString("o");
        }

        protected override DateTime GetUtcNow()
        {
            return this.utcNow;
        }
    }
}