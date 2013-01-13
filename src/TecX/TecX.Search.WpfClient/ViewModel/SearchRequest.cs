namespace TecX.Search.WpfClient.ViewModel
{
    using TecX.Search;

    public class SearchRequest
    {
        public SearchRequest()
        {
            this.SearchTerms = string.Empty;

            this.MaxResultCount = Defaults.MaxResultCount;
        }

        public string SearchTerms { get; set; }

        public int MaxResultCount { get; set; }
    }
}
