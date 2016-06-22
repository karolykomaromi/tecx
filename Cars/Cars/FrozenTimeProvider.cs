namespace Cars
{
    using System;

    public class FrozenTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset now;

        private readonly DateTimeOffset utcNow;

        public FrozenTimeProvider(DateTimeOffset now)
            : this(now.ToLocalTime(), now.ToUniversalTime())
        {
        }

        public FrozenTimeProvider(DateTimeOffset now, DateTimeOffset utcNow)
        {
            this.now = now;
            this.utcNow = utcNow;
        }

        protected override DateTimeOffset GetNow()
        {
            return this.now;
        }

        protected override DateTimeOffset GetUtcNow()
        {
            return this.utcNow;
        }
    }
}