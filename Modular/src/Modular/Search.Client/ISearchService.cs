namespace Search
{
    using System;
    using System.ServiceModel;
    using Search.Entities;

    [ServiceContract]
    public interface ISearchService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginSearchSuggestions(string searchTerm, AsyncCallback callback, object asyncState);

        string[] EndSearchSuggestions(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginSearch(string searchTerm, AsyncCallback callback, object asyncState);

        SearchResult[] EndSearch(IAsyncResult result);
    }

    public interface ISearchServiceChannel : ISearchService, IClientChannel
    {
    }
}