namespace Hydra.Infrastructure.Calendaring
{
    using System.Diagnostics.Contracts;

    public abstract class CalendarItem<T> : ICloneable<T>
        where T : class
    {
        public static implicit operator string(CalendarItem<T> item)
        {
            Contract.Requires(item != null);

            return item.ToString();
        }

        public abstract T Clone();

        public abstract override string ToString();
    }
}
