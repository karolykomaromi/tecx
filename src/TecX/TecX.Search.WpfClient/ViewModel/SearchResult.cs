namespace TecX.Search.WpfClient.ViewModel
{
    using System.Collections.Generic;

    public class SearchResult
    {
        public SearchResult()
        {
            this.Result = new Message[0];
        }

        public IEnumerable<Message> Result { get; set; }

        public int TotalRowsCount { get; set; }
    }
}
