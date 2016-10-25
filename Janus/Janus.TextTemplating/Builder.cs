using System.Diagnostics.Contracts;

namespace Janus.TextTemplating
{
    public abstract class Builder<T>
    {
        public static implicit operator T(Builder<T> builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<T>() != null);

            return builder.Build();
        }

        public abstract T Build();
    }
}