namespace Cars
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public interface IBuilder<T>
        where T : class
    {
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        int GetHashCode();
    }

    public abstract class Builder<T> : IBuilder<T>
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