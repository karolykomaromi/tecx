namespace Hydra.Infrastructure
{
    using System;

    public class FrozenTimeProvider : TimeProvider
    {
        private readonly DateTime now;

        private readonly DateTime utcNow;

        public FrozenTimeProvider(DateTime now)
            : this(now, now)
        {
        }

        public FrozenTimeProvider(DateTime now, DateTime utcNow)
        {
            this.now = now;
            this.utcNow = utcNow;
        }

        protected override DateTime GetNow()
        {
            return this.now;
        }

        protected override DateTime GetUtcNow()
        {
            return this.utcNow;
        }
    }
}