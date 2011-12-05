namespace TecX.Search.Data
{
    using System.Collections.Generic;

    public interface IMessageRepository
    {
        void AddMessage(Message message);

        IEnumerable<Message> Search(int maxResultCount, out int totalRowsCount, SearchParameterCollection searchParameters);

        void IndexMessagesForFullTextSearch();
    }
}
