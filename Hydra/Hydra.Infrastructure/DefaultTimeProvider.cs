namespace Hydra.Infrastructure
{
    using System;

    public class DefaultTimeProvider : TimeProvider
    {
        protected override DateTime GetNow()
        {
            return DateTime.Now;
        }

        protected override DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}