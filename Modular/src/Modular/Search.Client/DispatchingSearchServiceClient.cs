namespace Search
{
    using System;
    using System.Windows.Threading;
    using Search.Entities;

    public class DispatchingSearchServiceClient : ISearchService
    {
        private readonly ISearchService inner;

        private readonly Dispatcher dispatcher;

        public DispatchingSearchServiceClient(ISearchService inner, Dispatcher dispatcher)
        {
            this.inner = inner;
            this.dispatcher = dispatcher;
        }

        public IAsyncResult BeginSearchSuggestions(string searchTerm, AsyncCallback callback, object asyncState)
        {
            return this.inner.BeginSearchSuggestions(searchTerm, new Dispatched(this.dispatcher, callback).Callback, asyncState);
        }

        public string[] EndSearchSuggestions(IAsyncResult result)
        {
            return this.inner.EndSearchSuggestions(result);
        }

        public IAsyncResult BeginSearch(string searchTerm, AsyncCallback callback, object asyncState)
        {
            return this.inner.BeginSearch(searchTerm, new Dispatched(this.dispatcher, callback).Callback, asyncState);
        }

        public SearchResult[] EndSearch(IAsyncResult result)
        {
            return this.inner.EndSearch(result);
        }

        private class Dispatched
        {
            private readonly Dispatcher dispatcher;

            private readonly AsyncCallback callback;

            public Dispatched(Dispatcher dispatcher, AsyncCallback callback)
            {
                this.dispatcher = dispatcher;
                this.callback = callback;
            }

            public void Callback(IAsyncResult result)
            {
                this.dispatcher.BeginInvoke(() => this.callback(result));
            }
        }
    }
}