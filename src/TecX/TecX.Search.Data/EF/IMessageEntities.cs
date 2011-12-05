namespace TecX.Search.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    public interface IMessageEntities
    {
        IDbSet<Message> Messages { get; }

        IEnumerable<Message> SearchByInterfaceAndTimeFrame(int maxResultCount, out int totalRowsCount, string interfaceName, DateTime after, DateTime before);

        IEnumerable<Message> FindNonProcessedMessagesAndTagWithMarker();

        void AddSearchTerms(IEnumerable<SearchTerm> searchTerms);
    }
}