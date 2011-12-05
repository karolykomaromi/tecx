namespace TecX.Search.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Linq.Expressions;

    using TecX.Common;
    using TecX.Common.Extensions.LinqTo.Entities;
    using TecX.Search.Model;

    public class EFMessageRepository : IMessageRepository
    {
        private readonly IMessageEntities context;

        private readonly FullTextSearchTermProcessor processor;

        public EFMessageRepository(IMessageEntities context, FullTextSearchTermProcessor processor)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(processor, "processor");

            this.context = context;
            this.processor = processor;
        }

        public void AddMessage(Message message)
        {
            Guard.AssertNotNull(message, "message");

            this.context.Messages.Add(message);
        }

        public IEnumerable<Message> Search(int maxResultCount, out int totalRowsCount, SearchParameterCollection searchParameters)
        {
            if (SearchParameterHelper.IsInterfaceAndTimeFrameSearch(searchParameters))
            {
                SearchParameterHelper.ReorderForInterfaceAndTimeFrameSearch(searchParameters);

                return this.context.SearchByInterfaceAndTimeFrame(
                    maxResultCount,
                    out totalRowsCount,
                    (string)searchParameters[0].Value,
                    (DateTime)searchParameters[1].Value,
                    (DateTime)searchParameters[2].Value);
            }

            if (SearchParameterHelper.IsInterfaceAndDateSearch(searchParameters))
            {
                SearchParameterHelper.ReorderForInterfaceAndDateSearch(searchParameters);

                return this.context.SearchByInterfaceAndTimeFrame(
                    maxResultCount,
                    out totalRowsCount,
                    (string)searchParameters[0].Value,
                    (DateTime)searchParameters[1].Value,
                    SqlDateTime.MaxValue.Value);
            }

            if (SearchParameterHelper.IsTimeFrameSearch(searchParameters))
            {
                SearchParameterHelper.ReorderForTimeFrameSearch(searchParameters);

                return this.context.SearchByInterfaceAndTimeFrame(
                    maxResultCount,
                    out totalRowsCount,
                    "*",
                    (DateTime)searchParameters[0].Value,
                    (DateTime)searchParameters[1].Value);
            }

            if (SearchParameterHelper.IsInterfaceSearch(searchParameters))
            {
                return this.context.SearchByInterfaceAndTimeFrame(
                    maxResultCount,
                    out totalRowsCount,
                    (string)searchParameters[0].Value,
                    SqlDateTime.MinValue.Value,
                    SqlDateTime.MaxValue.Value);
            }

            Expression<Func<Message, bool>> where = msg => false;

            foreach (var parameter in searchParameters.OfType<SearchParameter<string>>())
            {
                // access to modified closure. we need to copy the search term to a variable that
                // is local to the scope of the loop and won't change when we iterate over the parameters
                string name = (string)parameter.Value;
                where = where.Or(msg => msg.Description.Contains(name));
            }

            var dates = searchParameters.OfType<SearchParameter<DateTime>>().OrderBy(p => p.Value).ToList();

            if (dates.Count > 2)
            {
                throw new ArgumentException(Constants.ErrorMessages.SearchTermsMustNotContainMoreThan2DateValues);
            }

            if (dates.Count > 0)
            {
                Expression<Func<Message, bool>> dateTimeWhere = msg => msg.SentAt >= (DateTime)dates[0].Value;

                if (dates.Count == 2)
                {
                    dateTimeWhere = dateTimeWhere.And(msg => msg.SentAt <= (DateTime)dates[1].Value);
                }

                where = dateTimeWhere.And(where);
            }

            totalRowsCount = this.context.Messages.Count(where);

            return this.context.Messages.Where(where).OrderByDescending(msg => msg.Id).Take(maxResultCount);
        }

        public void IndexMessagesForFullTextSearch()
        {
            var unprocessedMessages = this.context.FindNonProcessedMessagesAndTagWithMarker();

            IEnumerable<SearchTerm> searchTerms = this.processor.Analyze(unprocessedMessages);

            this.context.AddSearchTerms(searchTerms);
        }
    }
}
