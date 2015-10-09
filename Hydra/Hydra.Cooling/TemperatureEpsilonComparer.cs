namespace Hydra.Cooling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public abstract class TemperatureEpsilonComparer<TTemperature> : Comparer<TTemperature>
        where TTemperature : Temperature
    {
        private readonly TTemperature epsilon;

        protected TemperatureEpsilonComparer(TTemperature epsilon)
        {
            Contract.Requires(epsilon != null);

            this.epsilon = epsilon;
        }

        public sealed override int Compare(TTemperature x, TTemperature y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return 0;
            }

            if (object.ReferenceEquals(x, null))
            {
                return -1;
            }

            if (object.ReferenceEquals(y, null))
            {
                return 1;
            }

            decimal delta = Math.Abs(x.Value - y.Value);

            if (delta <= this.epsilon.Value)
            {
                return 0;
            }

            return x.Value.CompareTo(y.Value);
        }
    }
}