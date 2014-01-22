namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;

    public class LazyShowThings<T> : IShowThings<T>
        where T : class
    {
        private readonly Lazy<IShowThings<T>> lazy;

        public LazyShowThings(Func<IShowThings<T>> factory)
        {
            Contract.Requires(factory != null);

            this.lazy = new Lazy<IShowThings<T>>(factory);
        }

        public void Show(T thing)
        {
            this.lazy.Value.Show(thing);
        }
    }
}