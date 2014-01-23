namespace Search
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Search.Entities;

    public class SearchServiceClient : ClientBase<ISearchService>, ISearchService
    {
        public SearchServiceClient()
        {
        }

        public SearchServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public SearchServiceClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public SearchServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public SearchServiceClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public IAsyncResult BeginSearchSuggestions(string searchTerm, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginSearchSuggestions(searchTerm, callback, asyncState);
        }

        public string[] EndSearchSuggestions(IAsyncResult result)
        {
            return this.Channel.EndSearchSuggestions(result);
        }

        public IAsyncResult BeginSearch(string searchTerm, AsyncCallback callback, object asyncState)
        {
            return this.Channel.BeginSearch(searchTerm, callback, asyncState);
        }

        public SearchResult[] EndSearch(IAsyncResult result)
        {
            return this.Channel.EndSearch(result);
        }
    }
}
