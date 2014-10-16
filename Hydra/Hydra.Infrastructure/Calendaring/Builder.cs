using System.Diagnostics.Contracts;

namespace Hydra.Infrastructure.Calendaring
{
    public abstract class Builder<T>
        where T : class
    {
        public static implicit operator T(Builder<T> builder)
        {
            Contract.Requires(builder != null);

            return builder.Build();
        }

        public abstract T Build();
    }
}