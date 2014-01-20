namespace Search
{
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Search.Entities;

    [ServiceContract]
    [ServiceKnownType("AllEntities", typeof(KnownTypes))]
    [ContractClass(typeof(SearchServiceContract))]
    public interface ISearchService
    {
        [OperationContract]
        string[] SearchSuggestions(string searchTerm);

        [OperationContract]
        SearchResult[] Search(string searchTerm);
    }

    [ContractClassFor(typeof(ISearchService))]
    public abstract class SearchServiceContract : ISearchService
    {
        public string[] SearchSuggestions(string searchTerm)
        {
            Contract.Requires(!string.IsNullOrEmpty(searchTerm));
            Contract.Ensures(Contract.Result<string[]>() != null);

            return new string[0];
        }

        public SearchResult[] Search(string searchTerm)
        {
            Contract.Requires(!string.IsNullOrEmpty(searchTerm));
            Contract.Ensures(Contract.Result<SearchResult[]>() != null);

            return new SearchResult[0];
        }
    }
}
