namespace Search
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using System.Windows.Threading;
    using Search.Entities;

    public class SearchServiceClient : ClientBase<ISearchService>, ISearchService
    {
        private readonly Dispatcher dispatcher;

        public SearchServiceClient(Dispatcher dispatcher)
        {
            Contract.Requires(dispatcher != null);

            this.dispatcher = dispatcher;
        }

        public IAsyncResult BeginSearchSuggestions(string searchTerm, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginSearchSuggestions(searchTerm, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public string[] EndSearchSuggestions(IAsyncResult result)
        {
            return this.Channel.EndSearchSuggestions(result);
        }

        public IAsyncResult BeginSearch(string searchTerm, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginSearch(searchTerm, result => this.dispatcher.BeginInvoke(() => callback(result)), asyncState);
        }

        public SearchResult[] EndSearch(IAsyncResult result)
        {
            return this.Channel.EndSearch(result);
        }
    }
}
