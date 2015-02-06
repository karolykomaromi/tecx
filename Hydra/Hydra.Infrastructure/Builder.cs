namespace Hydra.Infrastructure
{
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public abstract class Builder<T>
        where T : class
    {
        public static implicit operator T(Builder<T> builder)
        {
            Contract.Requires(builder != null);

            return builder.Build();
        }

        public abstract T Build();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}