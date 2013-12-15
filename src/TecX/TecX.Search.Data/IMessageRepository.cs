namespace TecX.Search.Data
{
    using System.Collections.Generic;

    using TecX.Search.Model;

    public interface IMessageRepository
    {
        void AddMessage(Message message);

        IEnumerable<Message> Search(int maxResultCount, out int totalRowsCount, SearchParameterCollection searchParameters);

        void IndexMessagesForFullTextSearch();
    }
}
