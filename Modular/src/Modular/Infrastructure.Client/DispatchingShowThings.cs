namespace Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Windows.Threading;

    public class DispatchingShowThings<T> : IShowThings<T>
        where T : class
    {
        private readonly IShowThings<T> inner;
        private readonly Dispatcher dispatcher;

        public DispatchingShowThings(IShowThings<T> inner, Dispatcher dispatcher)
        {
            Contract.Requires(inner != null);
            Contract.Requires(dispatcher != null);

            this.inner = inner;
            this.dispatcher = dispatcher;
        }

        public void Show(T thing)
        {
            this.dispatcher.BeginInvoke(() => this.inner.Show(thing));
        }
    }
}