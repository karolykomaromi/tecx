namespace Search
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows.Threading;
    using Search.Service;

    public class DispatchingShowSearchResults : IShowSearchResults
    {
        private readonly IShowSearchResults inner;
        private readonly Dispatcher dispatcher;

        public DispatchingShowSearchResults(IShowSearchResults inner, Dispatcher dispatcher)
        {
            Contract.Requires(inner != null);
            Contract.Requires(dispatcher != null);

            this.inner = inner;
            this.dispatcher = dispatcher;
        }

        public void ShowSearchResults(IEnumerable<SearchResult> results)
        {
            Contract.Requires(results != null);

            this.dispatcher.BeginInvoke(() => this.inner.ShowSearchResults(results));
        }
    }
}